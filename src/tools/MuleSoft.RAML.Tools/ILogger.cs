using System;

namespace MuleSoft.RAML.Tools
{
    public interface ILogger
    {
        void LogError(Exception ex);
        void LogInformation(string message);
    }
}