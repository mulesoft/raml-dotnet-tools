using System;

namespace MuleSoft.RAML.Tools
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