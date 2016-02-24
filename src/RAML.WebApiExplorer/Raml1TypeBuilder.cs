using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;

namespace RAML.WebApiExplorer
{
    public class Raml1TypeBuilder : TypeBuilderBase
    {
        private readonly MyOrderedDictionary raml1Types;

        public Raml1TypeBuilder(MyOrderedDictionary raml1Types)
        {
            this.raml1Types = raml1Types;
        }

        public string Add(Type type, int pad = 0)
        {
            var typeName = type.Name.Replace("`", string.Empty);
            if(Types.Contains(type))
                return typeName;

            string raml1Type;

            if (type.IsGenericType && IsGenericWebResult(type))
                type = type.GetGenericArguments()[0];

            if (IsArrayOrEnumerable(type))
            {
                var elementType = GetElementType(type);
                if (IsArrayOrEnumerable(elementType))
                {
                    var subElementType = GetElementType(elementType);
                    if (!HasPropertiesOrParentType(subElementType))
                        return string.Empty;

                    raml1Type = GetArrayOfArray(subElementType);
                }
                else
                {
                    if (!HasPropertiesOrParentType(elementType))
                        return string.Empty;

                    raml1Type = GetArray(elementType);                    
                }
            }
            else if (IsDictionary(type))
            {
                raml1Type = GetMap(type);
            }
            else if (type.IsPrimitive)
            {
                raml1Type = GetScalar(type);
            }
            else if (type.IsEnum)
            {
                raml1Type = GetEnum(type);
            }
            else
            {
                if (!HasPropertiesOrParentType(type))
                    return string.Empty;

                raml1Type = GetObject(type);
            }

            AddType(type, raml1Type);

            return typeName;
        }

        private string GetMap(Type type)
        {
            var subtype = type.GetGenericArguments()[1];
            var subtypeName = subtype.Name;

            if(!subtype.IsPrimitive)
                subtypeName = Add(subtype, 4);

            var raml1Type = "type: object" + Environment.NewLine;
            raml1Type += "properties:" + Environment.NewLine;
            raml1Type += "[]:".Indent(4) + Environment.NewLine;
            
            raml1Type += "type: ".Indent(8) + subtypeName;
            return raml1Type;
        }

        private string GetArrayOfArray(Type subElementType)
        {
            var elementTypeName = subElementType.Name;
            if (!subElementType.IsPrimitive)
                elementTypeName = Add(subElementType);

            return "type: " + elementTypeName + "[][]";
        }

        private string GetArray(Type elementType)
        {
            var elementTypeName = elementType.Name;
            if (!elementType.IsPrimitive)
                elementTypeName = Add(elementType);

            return "type: " + elementTypeName + "[]";
        }

        private string GetScalar(Type type)
        {
            return "type:" + Raml1TypeMapper.Map(type);
        }

        private string GetEnum(Type type)
        {
            return "type: string" + Environment.NewLine +
                   "enum: [ " + string.Join(",",type.GetEnumValues()) + " ]";
        }

        private string GetObject(Type type)
        {
            var raml1Type = string.Empty;

            if (!type.IsSubclassOf(typeof(Object)) && type.BaseType != null)
            {
                var parent = GetObject(type.BaseType);
                AddType(type.BaseType, parent);

                raml1Type = "type:" + type.BaseType + Environment.NewLine;
            }

            if (type.GetProperties().Count(p => p.CanRead || p.CanWrite) > 0)
            {
                raml1Type += "properties:" + Environment.NewLine;
                raml1Type += GetProperties(type, 4);
            }
            return raml1Type;
        }

        protected override string HandleEnumProperty(int pad, PropertyInfo prop, IEnumerable<PropertyInfo> props, string schema)
        {
            schema += GetPropertyName(prop).Indent(pad);
            schema += GetEnum(prop.PropertyType).Indent(pad + 4);
            return schema;
        }

        protected override string HandlePrimitiveTypeProperty(int pad, PropertyInfo prop, IEnumerable<PropertyInfo> props, string schema,
            IEnumerable<CustomAttributeData> customAttributes)
        {
            schema += GetPropertyName(prop).Indent(pad);

            if (IsOptionalProperty(prop, customAttributes))
                schema += "?";

            schema += ":" + Environment.NewLine;

            schema += GetScalar(prop.PropertyType).Indent(pad + 4);
            schema += HandleValidationAttributes(customAttributes).Indent(pad + 8);

            return schema;
        }

        private static bool IsOptionalProperty(PropertyInfo prop, IEnumerable<CustomAttributeData> customAttributes)
        {
            return customAttributes.All(a => a.AttributeType != typeof(RequiredAttribute)) && IsNullable(prop.PropertyType);
        }

        protected override string HandleNestedTypeProperty(int pad, PropertyInfo prop, string schema, IEnumerable<PropertyInfo> props,
            IEnumerable<CustomAttributeData> customAttributes)
        {
            var typeName = Add(prop.PropertyType, pad + 4);

            schema += GetPropertyName(prop).Indent(pad) + Environment.NewLine;

            if (IsOptionalProperty(prop, customAttributes))
                schema += "?";

            schema += ": " + typeName + Environment.NewLine;

            return schema;
        }

        protected override string HandleValidationAttribute(CustomAttributeData attribute)
        {
            var res = string.Empty;

            switch (attribute.AttributeType.Name)
            {
                case "MaxLengthAttribute":
                    res += Environment.NewLine + "maxLength: " + attribute.ConstructorArguments.First().Value;
                    break;
                case "MinLengthAttribute":
                    res += Environment.NewLine + "minLength: " + attribute.ConstructorArguments.First().Value;
                    break;
                case "RangeAttribute":
                    if (!IsMinValue(attribute.ConstructorArguments.First()))
                        res += Environment.NewLine + "minimum: " + Format(attribute.ConstructorArguments.First());
                    if (!IsMaxValue(attribute.ConstructorArguments.Last()))
                        res += Environment.NewLine + "maximum: " + Format(attribute.ConstructorArguments.Last());
                    break;
                case "EmailAddressAttribute":
                    res += Environment.NewLine + @"pattern: [^\\s@]+@[^\\s@]+\\.[^\\s@]";
                    break;
                case "UrlAttribute":
                    res += Environment.NewLine + @"pattern: ^(ftp|http|https):\/\/[^ \""]+$";
                    break;
                //case "RegularExpressionAttribute":
                //    res += "pattern: " + " + attribute.ConstructorArguments.First().Value;
                //    break;
            }
            return res;
        }

        private static Type GetElementType(Type type)
        {
            return type.GetElementType() ?? type.GetGenericArguments()[0];
        }

        private static bool IsDictionary(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>) || type.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        private static bool HasPropertiesOrParentType(Type type)
        {
            return type.BaseType != null || type.GetProperties().Any(p => p.CanRead || p.CanWrite);
        }

        private void AddType(Type type, string raml1Type)
        {
            if(IsArrayOrEnumerable(type)) // TODO: do not save array types, check !!
                return;

            var typeName = IsDictionary(type) ? type.GetGenericArguments()[1].Name + "Map" : type.Name;

            // handle case of different types with same class name
            if (raml1Types.ContainsKey(typeName))
                typeName = GetUniqueSchemaName(typeName);

            raml1Types.Add(typeName, raml1Type);
        }

        protected string GetUniqueSchemaName(string schemaName)
        {
            for (var i = 0; i < 1000; i++)
            {
                schemaName += i;
                if (!raml1Types.ContainsKey(schemaName))
                    return schemaName;
            }
            throw new InvalidOperationException("Could not find a unique name. You have more than 1000 types with the same class name");
        }


    }
}