using System;

namespace AMF.Tools
{
    public static class UserInfo
    {
        public static string Get()
        {
            var username = Environment.UserName;
            var machine = Environment.MachineName;
            var userId = username + "@" + machine;
            return userId;
        }

        public static string GetPlatform()
        {
            var os = Environment.OSVersion;
            return os.VersionString;
        }
    }
}
