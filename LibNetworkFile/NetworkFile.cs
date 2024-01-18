using Microsoft.Win32.SafeHandles;
using SimpleImpersonation;
using System.IO.Compression;
using System.Security.Principal;
using System.Text;

namespace LibNetworkFile
{
    public class NetworkFileHelper
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static void SaveBase64ToNetwork(string domain, string userName, string password
                                        , string base64string
                                        , string destinationFile
                                        , bool replaceExisting = true)
        {
            var logonType = LogonType.NewCredentials;
            var credentials = new UserCredentials(domain, userName, password);
            using (SafeAccessTokenHandle userHandle = credentials.LogonUser(logonType))
            {
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    // do whatever you want as this user.
                    if (replaceExisting)
                    {
                        try
                        {
                            var dir = Path.GetDirectoryName(destinationFile) ?? "";
                            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
                            if (File.Exists(destinationFile)) File.Delete(destinationFile);
                        }
                        finally
                        {

                        }
                    }
                    File.WriteAllBytes(destinationFile, Convert.FromBase64String(base64string));
                });
            }
            //Impersonation.RunAsUser(credentials, logonType, () =>
            //{
            //    if (replaceExisting)
            //    {
            //        try
            //        {
            //            var dir = Path.GetDirectoryName(destinationFile);
            //            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            //            if (File.Exists(destinationFile)) File.Delete(destinationFile);
            //        }
            //        finally
            //        {

            //        }
            //    }
            //    File.WriteAllBytes(destinationFile, Convert.FromBase64String(base64string));
            //});
        }

        public static string GetBase64FromNetwork(string domain, string userName,
                                                    string password,
                                                    string destinationFile)
        {
            var logonType = LogonType.NewCredentials;
            var credentials = new UserCredentials(domain, userName, password);
            string result = string.Empty;
            using (SafeAccessTokenHandle userHandle = credentials.LogonUser(logonType))
            {
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    try
                    {
                        if (File.Exists(destinationFile))
                        {
                            result = Convert.ToBase64String(File.ReadAllBytes(destinationFile));
                        }
                    }
                    finally
                    {

                    }
                });

                //        Impersonation.RunAsUser(credentials, logonType, () =>
                //{
                //    try
                //    {
                //        if (File.Exists(destinationFile))
                //        {
                //            result = Convert.ToBase64String(File.ReadAllBytes(destinationFile));
                //        }
                //    }
                //    finally
                //    {

                //    }
                //});
            }
            return result;
        }

        public static void CopyOverNetwork(string domain, string userName, string password
                                            , string source
                                            , string destination
                                            , bool deleteSourceWhenDone = false)
        {
            var logonType = LogonType.NewCredentials;
            var credentials = new UserCredentials(domain, userName, password);
            using (SafeAccessTokenHandle userHandle = credentials.LogonUser(logonType))
            {
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    try
                    {
                        var dir = Path.GetDirectoryName(destination) ?? "";
                        if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
                        File.Copy(source, destination, true);
                        if (deleteSourceWhenDone)
                        {
                            File.Delete(source);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {

                    }
                });
            }
            //    Impersonation.RunAsUser(credentials, logonType, () =>
            //{


            //    }
        }


        public static void SaveFileOverNetwork(string domain, string userName, string password
                                                , Stream fileStream
                                                , string destination)
        {
            if (fileStream != null && fileStream.Length > 0)
            {
                var logonType = LogonType.NewCredentials;
                var credentials = new UserCredentials(domain, userName, password);
                using (SafeAccessTokenHandle userHandle = credentials.LogonUser(logonType))
                {
                    WindowsIdentity.RunImpersonated(userHandle, () =>
                    {
                        try
                        {
                            var dir = Path.GetDirectoryName(destination) ?? "";
                            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
                            using (var fi = File.Create(destination))
                            {
                                fileStream.Seek(0, SeekOrigin.Begin);
                                fileStream.CopyTo(fi);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {

                        }
                    });
                }
                //    Impersonation.RunAsUser(credentials, logonType, () =>
                //{
                //    try
                //    {
                //        var dir = Path.GetDirectoryName(destination);
                //        if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
                //        using (var fi = File.Create(destination))
                //        {
                //            fileStream.Seek(0, SeekOrigin.Begin);
                //            fileStream.CopyTo(fi);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
                //    finally
                //    {

                //    }
                //});
            }
            else
            {
                throw new Exception("Cannot read file stream.");
            }
        }

        public static bool IsFileExists(string domain, string userName, string password, string destination)
        {
            bool result = false;
            var logonType = LogonType.NewCredentials;
            var credentials = new UserCredentials(domain, userName, password);
            using (SafeAccessTokenHandle userHandle = credentials.LogonUser(logonType))
            {
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    //Impersonation.RunAsUser(credentials, logonType, () =>
                    //{
                    try
                    {
                        result = File.Exists(destination);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {

                    }
                });
            }
            return result;
        }

        public static Stream ReadFile(string domain, string userName, string password, string destination)
        {
            var logonType = LogonType.NewCredentials;
            var credentials = new UserCredentials(domain, userName, password);
            Stream result = Stream.Null;
            using (SafeAccessTokenHandle userHandle = credentials.LogonUser(logonType))
            {
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    //Impersonation.RunAsUser(credentials, logonType, () =>
                    //{
                    try
                    {
                        result = File.OpenRead(destination);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {

                    }
                });
            }
            return result;
        }

        public static byte[]? GetBytesFromNetwork(string domain, string userName, string password
                                               , string fullFileName)
        {
            var logonType = LogonType.NewCredentials;
            var credentials = new UserCredentials(domain, userName, password);
            byte[]? result = default;
            using (SafeAccessTokenHandle userHandle = credentials.LogonUser(logonType))
            {
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    //Impersonation.RunAsUser(credentials, logonType, () =>
                    //{
                    try
                    {
                        if (File.Exists(fullFileName))
                        {
                            result = File.ReadAllBytes(fullFileName);
                        }
                    }
                    finally
                    {

                    }
                });
            }
            return result;
        }

        public static void CopyFolder(string domain, string userName, string password
                                        , string source, string destination
                                        , bool deleteSourceWhenDone = false)
        {
            var logonType = LogonType.NewCredentials;
            var credentials = new UserCredentials(domain, userName, password);
            using (SafeAccessTokenHandle userHandle = credentials.LogonUser(logonType))
            {
                WindowsIdentity.RunImpersonated(userHandle, () =>
                {
                    //Impersonation.RunAsUser(credentials, logonType, () =>
                    //{
                    try
                    {
                        var dir = destination;
                        if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

                        foreach (string filename in Directory.EnumerateFiles(source))
                        {
                            File.Copy(filename, Path.Combine(destination, Path.GetFileName(filename)));
                            if (deleteSourceWhenDone)
                            {
                                File.Delete(filename);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {

                    }
                });
            }
        }

        public static void ExtractZipBase64FromNetwork(string domain, string userName, string password
                                        , string base64string, string destination
                                        , bool Overfile = false)
        {

            var logonType = LogonType.NewCredentials;
            var credentials = new UserCredentials(domain, userName, password);

            using (SafeAccessTokenHandle userHandle = credentials.LogonUser(logonType))
            {
                try
                {
                    WindowsIdentity.RunImpersonated(userHandle, () =>
                    {
                        if (!Directory.Exists(destination))
                        {
                            Directory.CreateDirectory(destination);
                        }
                        byte[] zipBytes = Convert.FromBase64String(base64string);
                        using(var memoryStream = new MemoryStream(zipBytes))
                        {
                            using(var arc = new ZipArchive(memoryStream))
                            {
                                arc.ExtractToDirectory(destination, Overfile);
                            }
                        }

                    });
                }
                catch (Exception)
                {
                    throw;
                }
                finally{

                }
            }
        }

    }
}