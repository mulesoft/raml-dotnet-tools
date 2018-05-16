using System;

namespace AMF.Tools
{
    public interface ILogger
    {
        void LogError(Exception ex);
        void LogInformation(string message);
    }
}