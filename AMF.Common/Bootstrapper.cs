using System.Collections.Generic;
using System.Reflection;
using Caliburn.Micro;
using AMF.Common.ViewModels;

namespace AMF.Common
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper() : base(false)
        {
            Initialize();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[]
            {
                new RamlChooserViewModel().GetType().Assembly
            };
        }
    }
}