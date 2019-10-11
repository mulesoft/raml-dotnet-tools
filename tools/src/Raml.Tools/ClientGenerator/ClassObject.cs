using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Raml.Tools.ClientGenerator
{
    [Serializable]
    public class ClassObject
    {
        public ClassObject()
        {
            Methods = new Collection<ClientGeneratorMethod>();
            Children = new Collection<ClassObject>();
            Properties = new Collection<FluentProperty>();
        }

        public string Name { get; set; }
        public ICollection<ClientGeneratorMethod> Methods { get; set; }
        public ICollection<ClassObject> Children { get; set; }
        public ICollection<FluentProperty> Properties { get; set; }
        public string Description { get; set; }
    }
}