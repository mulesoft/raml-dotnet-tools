using System;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using AMF.Tools.Properties;
using AMF.Common;
using Microsoft.VisualStudio.ComponentModelHost;
using NuGet.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft;

namespace AMF.Tools
{
    public class RamlScaffoldServiceAspNetCore3 : RamlScaffoldServiceAspNetCore
    {
        public RamlScaffoldServiceAspNetCore3(IT4Service t4Service, IServiceProvider serviceProvider): base(t4Service, serviceProvider){}

        public override string TemplateSubFolder
        {
            get { return "AspNetCore3"; }
        }

    }
}