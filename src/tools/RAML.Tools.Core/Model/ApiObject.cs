using System;
using System.Collections.Generic;
using System.Linq;

namespace AMF.Tools.Core
{
    [Serializable]
    public class ApiObject : Core.WebApiGenerator.IHasName
    {
        public ApiObject()
        {
            Properties = new List<Property>();
        }

        public Guid Id { get; set; }
        public string AmfId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
        public IList<Property> Properties { get; set; }

        public Property GetContentOrDefault()
        {
            return Properties.FirstOrDefault(p => p.Name == "Content"); 
        }

        public bool IsArray { get; set; }
        public bool IsMap { get; set; }
        public bool IsMultiple { get; set; }

        public bool IsUnionType { get; set; }

        public string JSONSchema { get; set; }

	    public string GeneratedCode { get; set; }

        public string BaseClass { get; set; }

        public string ConstructorParams
        {
            get
            {
                var res = string.Empty;
                if (Properties.Any(p => p.Name == "UriParameters"))
                {
                    var uriParams = Properties.First(p => p.Name == "UriParameters");
                    res += uriParams.Type + " " + uriParams.Name;
                }

                var paramStrings = Properties
                    .Where(p => p != null && p.Type != null && p.Name != null && p.Name != "UriParameters" && NewNetTypeMapper.IsNullableType(p.Type))
                    .Select(p => p.Type + " " + p.Name + " = null").ToArray();

                if (!paramStrings.Any())
                    return res;

                return res + (string.IsNullOrWhiteSpace(res) ? "" : ", ") + string.Join(", ", paramStrings);
            }
        }

        public string Type { get; set; }
        public bool IsScalar { get; set; }

        //public void SetUnionType(object content)
        //{
        //    if (!IsUnionType)
        //        throw new InvalidOperationException("GetUnionRuntimeType can only be used with Union Types");

        //    var properties = GetType().GetProperties();
        //    foreach (var property in properties)
        //    {
        //        if (content.GetType() == property.PropertyType)
        //            property.SetValue(this, content);
        //    }
        //}
    }
}