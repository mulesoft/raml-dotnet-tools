using System;
using Microsoft.VisualStudio.Shell;
using Raml.Common;

namespace MuleSoft.RAML.Tools
{
    public class ActivityLogger : ILogger
    {
        public void LogError(Exception ex)
        {
            ActivityLog.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, VisualStudioAutomationHelper.GetExceptionInfo(ex));
        }

        public void LogInformation(string message)
        {
            ActivityLog.LogInformation(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, message);
        }        
    }
}