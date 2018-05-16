using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AMF.Tools.Core.WebApiGenerator;

namespace AMF.Tools.Core
{
    [Serializable]
    public class ApiEnum : Core.WebApiGenerator.IHasName
    {
        public ApiEnum()
        {
            Values = new Collection<PropertyBase>();
        }
        public string Name { get; set; }
        public ICollection<PropertyBase> Values { get; set; }
        public string Description { get; set; }
    }
}