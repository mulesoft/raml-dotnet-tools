using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Raml.Tools.WebApiGenerator
{
    [Serializable]
    public class ControllerObject : IHasName
    {
        public ControllerObject()
        {
            Methods = new Collection<ControllerMethod>();
        }

        public string Name { get; set; }
        public string PrefixUri { get; set; }
        public ICollection<ControllerMethod> Methods { get; set; }
        public string Description { get; set; }
    }
}