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

        public string Add(Type type)
        {
            var typeName = type.Name.Replace("`", string.Empty);
            if(Types.Contains(type))
                return typeName;

            Types.Add(type);

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
            else if (type.IsEnum)
            {
                raml1Type = GetEnum(type);
            }
            else if (Raml1TypeMapper.Map(type) != null)
            {
                raml1Type = GetScalar(type);
            }
            else
            {
                if (!HasPropertiesOrParentType(type))
                    return string.Empty;

                raml1Type = GetObject(type);
            }

            if(!string.IsNullOrWhiteSpace(raml1Type))
                AddType(type, raml1Type);

            return typeName;
        }

        private string GetMap(Type type)
        {
            var subtype = type.GetGenericArguments()[1];
            var subtypeName = subtype.Name;

            if(Raml1TypeMapper.Map(subtype) == null)
                subtypeName = Add(subtype);

            if(string.IsNullOrWhiteSpace(subtypeName))
                return string.Empty;

            var raml1Type = "type: object" + Environment.NewLine;
            raml1Type += "properties:" + Environment.NewLine;
            raml1Type += "[]:".Indent(4) + Environment.NewLine;
            
            raml1Type += "type: ".Indent(8) + subtypeName;
            return raml1Type;
        }

        private string GetArrayOfArray(Type subElementType)
        {
            var elementTypeName = subElementType.Name;
            if (Raml1TypeMapper.Map(subElementType) == null)
                elementTypeName = Add(subElementType);

            if(string.IsNullOrWhiteSpace(elementTypeName))
                return string.Empty;

            return "type: " + elementTypeName + "[][]";
        }

        private string GetArray(Type elementType)
        {
            var elementTypeName = elementType.Name;
            if (Raml1TypeMapper.Map(elementType) == null)
                elementTypeName = Add(elementType);

            if (string.IsNullOrWhiteSpace(elementTypeName))
                return string.Empty;

            return "type: " + elementTypeName + "[]";
        }

        private string GetScalar(Type type)
        {
            return "type:" + Raml1TypeMapper.Map(type);
        }

        private string GetEnum(Type type)
        {
            return "type: string" + Environment.NewLine +
                   "enum: [ " + string.Join(",",type.GetEnumNames()) + " ]";
        }

        private string GetObject(Type type)
        {
            var raml1Type = string.Empty;

            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                var parent = GetObject(type.BaseType);
                AddType(type.BaseType, parent);

                raml1Type = "type: " + type.BaseType.Name + Environment.NewLine;
            }

            if (type.GetProperties().Count(p => p.CanWrite) > 0)
            {
                raml1Type += "properties:" + Environment.NewLine;
                raml1Type += GetProperties(type, 4);
            }
            return raml1Type;
        }

        protected override string GetProperty(int pad, PropertyInfo prop, string schema, IEnumerable<PropertyInfo> props, 
            IEnumerable<CustomAttributeData> customAttributes)
        {
            if (prop.PropertyType.IsEnum)
                schema = HandleEnumProperty(pad, prop, props, schema);
            else if (Raml1TypeMapper.Map(prop.PropertyType) != null)
                schema = HandlePrimitiveTypeProperty(pad, prop, props, schema, customAttributes);
            else if (IsArrayOrEnumerable(prop.PropertyType))
                schema = HandleArrayProperty(pad, prop, schema, props, customAttributes);
            else if (IsDictionary(prop.PropertyType))
                schema = HandleDictionaryProperty(pad, prop, schema, props, customAttributes);
            else
                schema = HandleNestedTypeProperty(pad, prop, schema, props, customAttributes);

            return schema;
        }

        private string HandleArrayProperty(int pad, PropertyInfo prop, string schema, IEnumerable<PropertyInfo> props, IEnumerable<CustomAttributeData> customAttributes)
        {
            schema += GetPropertyName(prop).Indent(pad) + ":";
            schema += GetArray(prop.PropertyType).Indent(pad + 4) + Environment.NewLine;
            return schema;
        }

        private string HandleDictionaryProperty(int pad, PropertyInfo prop, string schema, IEnumerable<PropertyInfo> props, IEnumerable<CustomAttributeData> customAttributes)
        {
            schema += GetPropertyName(prop).Indent(pad) + ":";
            schema += GetMap(prop.PropertyType).Indent(pad + 4) + Environment.NewLine;
            return schema;
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

            schema += GetScalar(prop.PropertyType).Indent(pad + 4) + Environment.NewLine;
            schema += HandleValidationAttributes(customAttributes).Indent(pad + 8);

            return schema;
        }

        protected override string HandleNestedTypeProperty(int pad, PropertyInfo prop, string schema, IEnumerable<PropertyInfo> props,
            IEnumerable<CustomAttributeData> customAttributes)
        {
            var typeName = Add(prop.PropertyType);

            if (string.IsNullOrWhiteSpace(typeName))
                return string.Empty;

            schema += GetPropertyName(prop).Indent(pad);
            if (IsOptionalProperty(prop, customAttributes))
                schema += "?";
            schema += ":" + Environment.NewLine;

            schema += "type: ".Indent(pad + 4) + typeName + Environment.NewLine;

            return schema;
        }

        private static bool IsOptionalProperty(PropertyInfo prop, IEnumerable<CustomAttributeData> customAttributes)
        {
            return customAttributes.All(a => a.AttributeType != typeof(RequiredAttribute)) && IsNullable(prop.PropertyType);
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
            return (type.BaseType != null && type.BaseType != typeof(object)) || type.GetProperties().Any(p => p.CanWrite);
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