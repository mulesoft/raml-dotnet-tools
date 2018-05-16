using System;

namespace AMF.Tools
{
    public class NullLogger : ILogger
    {
        public void LogError(Exception ex)
        {
            throw ex;
        }

        public void LogInformation(string message)
        {
        }
    }
}