using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AMF.Tools.Core
{
    [Serializable]
    public class ApiEnum : Core.WebApiGenerator.IHasName
    {
        public ApiEnum()
        {
            Values = new Collection<PropertyBase>();
        }
        public Guid Id { get; set; }
        public string AmfId { get; set; }
        public string Name { get; set; }
        public ICollection<PropertyBase> Values { get; set; }
        public string Description { get; set; }
    }
}