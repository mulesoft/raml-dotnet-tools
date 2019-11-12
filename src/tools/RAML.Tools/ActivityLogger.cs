using System;
using AMF.Common;
using Microsoft.VisualStudio.Shell;

namespace AMF.Tools
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