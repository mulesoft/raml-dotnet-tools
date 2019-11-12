using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;

namespace AMF.Common
{
    public static class FolderHelper
    {
        public static readonly string TempPath = "./Temp";

        public static void CreateFolderIfNotPresent(string tempPath)
        {
            var currentUser = WindowsIdentity.GetCurrent();
            var security = new DirectorySecurity();
            security.AddAccessRule(new FileSystemAccessRule(currentUser.User, FileSystemRights.Write, AccessControlType.Allow));
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath, security);
            }
            else
            {
                var directoryInfo = new DirectoryInfo(tempPath);
                var directorySecurity = directoryInfo.GetAccessControl();
                directorySecurity.AddAccessRule(new FileSystemAccessRule(currentUser.User, FileSystemRights.Write, AccessControlType.Allow));
                directoryInfo.SetAccessControl(directorySecurity);
            }
        }

        public static void SetCorrectPermissions(string path)
        {
            var currentUser = WindowsIdentity.GetCurrent();
            var security = new FileSecurity();
            security.AddAccessRule(new FileSystemAccessRule(currentUser.User, FileSystemRights.Write, AccessControlType.Allow));
            if (!File.Exists(path))
            {
                File.Create(path, 50, FileOptions.None, security);
            }
            else
            {
                var file = new FileInfo(path);
                file.SetAccessControl(security);
            }
        }
    }
}