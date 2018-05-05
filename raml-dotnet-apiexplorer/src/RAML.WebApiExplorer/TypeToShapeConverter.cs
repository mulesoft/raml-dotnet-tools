using AMF.Parser.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace RAML.WebApiExplorer
{
    public class TypeToShapeConverter
    {
        public IDictionary<string, Shape> Types { get; }

        public TypeToShapeConverter()
        {
            Types = new Dictionary<string, Shape>();
        }

        public Shape Convert(string primitiveType)
        {
            return new ScalarShape(primitiveType, null, 0, 0, null, null, null, null, null, 0, null, null, null, null, null, null, null, null, null, null);
        }

        public Shape Convert(Type type)
        {
            if (Types.ContainsKey(type.FullName))
                return Types[type.FullName];

            if (type.IsGenericType && TypeBuilderHelper.IsGenericWebResult(type))
                type = type.GetGenericArguments()[0];

            if (primitiveConversion.ContainsKey(type))
            {
                var convertedType = primitiveConversion[type];
                return new ScalarShape(convertedType, null, 0, 0, null, null, null, null, null, 0, null, null, null, null, null, null, null, null, null, null);
            }

            var inherits = new List<Shape>();
            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                inherits.Add(Convert(type.BaseType));
            }

            if (TypeBuilderHelper.IsArrayOrEnumerable(type))
            {
                var elementType = GetElementType(type);
                Shape itemsShape = Convert(elementType);
                var arrayShape = new ArrayShape(itemsShape, 0, 0, false, null, null, null, null, null, null, null, null, inherits, null);
                Types.Add(type.FullName, arrayShape);
                return arrayShape;
            }

            if (IsDictionary(type) || IsParentADictionary(type))
            {
                //TODO: check
                //shape = GetMap(type);
                //types.Add(type.FullName, shape);
                //return shape;
            }

            if (type.IsEnum)
            {
                //TODO: check
                //types.Add(type.FullName, shape);
                //shape = GetEnum(type);
                //return shape;
            }

            if (!HasPropertiesOrParentType(type))
                return null;

            IList<PropertyShape> properties = new List<PropertyShape>();
            if (GetClassProperties(type).Count(p => p.CanWrite) > 0)
            {
                properties = GetProperties(type);
            }

            var shape = new NodeShape(0, 0, false, string.Empty, string.Empty, false, properties, null, null, null, null, type.FullName, null, null, null,
                null, inherits, null);

            Types.Add(type.FullName, shape);

            return shape;
        }

        private void ConvertWithoutAdding(Type elementType)
        {
            throw new NotImplementedException();
        }

        private IList<PropertyShape> GetProperties(Type type)
        {
            var props = GetClassProperties(type).ToArray();
            var properties = new List<PropertyShape>();
            foreach (var prop in props)
            {
                var shape = Convert(prop.PropertyType);
                if (shape != null)
                {
                    if(shape is ScalarShape scalar && prop.CustomAttributes != null && prop.CustomAttributes.Any())
                        shape = HandleValidationAttributes(scalar, prop.CustomAttributes);

                    var property = new PropertyShape(prop.Name, shape, 0, 0);
                    properties.Add(property);
                }
            }
            return properties;
        }

        private static IEnumerable<PropertyInfo> GetClassProperties(Type type)
        {
            var properties = type.GetProperties().Where(p => p.CanWrite);
            if (type.BaseType != null && type.BaseType != typeof(Object))
            {
                var parentProperties = type.BaseType.GetProperties().Where(p => p.CanWrite);
                properties = properties.Where(p => parentProperties.All(x => x.Name != p.Name));
            }
            return properties;
        }


        private static bool IsParentADictionary(Type type)
        {
            return (type.BaseType != null && type.BaseType != typeof(Object) && IsDictionary(type.BaseType));
        }

        private static bool IsOptionalProperty(PropertyInfo prop, IEnumerable<CustomAttributeData> customAttributes)
        {
            return customAttributes.All(a => a.AttributeType != typeof(RequiredAttribute)) && TypeBuilderHelper.IsNullable(prop.PropertyType);
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

        private IDictionary<Type, string> primitiveConversion = new Dictionary<Type, string>
        {
            { typeof(decimal), "http://www.w3.org/2001/XMLSchema#decimal" },
            { typeof(float), "http://www.w3.org/2001/XMLSchema#float" },
            { typeof(double), "http://www.w3.org/2001/XMLSchema#double" },
            { typeof(int), "http://www.w3.org/2001/XMLSchema#integer" },
            { typeof(long), "http://www.w3.org/2001/XMLSchema#long" },
            //{ typeof(int), "http://www.w3.org/2001/XMLSchema#int" },
            { typeof(short), "http://www.w3.org/2001/XMLSchema#short" },
            { typeof(byte), "http://www.w3.org/2001/XMLSchema#byte" },
            { typeof(ulong), "http://www.w3.org/2001/XMLSchema#unsignedLong" },
            { typeof(uint), "http://www.w3.org/2001/XMLSchema#unsignedInt" },
            { typeof(ushort), "http://www.w3.org/2001/XMLSchema#unsignedShort" },
            { typeof(DateTime), "http://www.w3.org/2001/XMLSchema#dateTime" },
            { typeof(TimeSpan), "http://www.w3.org/2001/XMLSchema#duration" }, //TODO: check
            { typeof(string), "http://www.w3.org/2001/XMLSchema#string" },
            { typeof(bool), "http://www.w3.org/2001/XMLSchema#boolean" }
        };

        private ScalarShape HandleValidationAttributes(ScalarShape prop, IEnumerable<CustomAttributeData> customAttributes)
        {
            foreach (var attribute in customAttributes)
            {
                prop = HandleValidationAttribute(prop, attribute);
            }
            return prop;
        }

        private static ScalarShape HandleValidationAttribute(ScalarShape prop, CustomAttributeData attribute)
        {
            int? propMaxLength = prop.MaxLength;
            int? propMinLength = prop.MinLength;
            string propMinimum = prop.Minimum;
            string propMaximum = prop.Maximum;
            string propPattern = prop.Pattern;

            switch (attribute.AttributeType.Name)
            {
                case "MaxLengthAttribute":
                    propMaxLength = (int)attribute.ConstructorArguments.First().Value;
                    break;
                case "MinLengthAttribute":
                    propMinLength = (int)attribute.ConstructorArguments.First().Value;
                    break;
                case "RangeAttribute":
                    if (!TypeBuilderHelper.IsMinValue(attribute.ConstructorArguments.First()))
                        propMinimum = attribute.ConstructorArguments.First().Value?.ToString();
                    if (!TypeBuilderHelper.IsMaxValue(attribute.ConstructorArguments.Last()))
                        propMaximum = attribute.ConstructorArguments.Last().Value?.ToString();
                    break;
                case "EmailAddressAttribute":
                    propPattern = @"pattern: [^\\s@]+@[^\\s@]+\\.[^\\s@]";
                    break;
                case "UrlAttribute":
                    propPattern = @"pattern: ^(ftp|http|https):\/\/[^ \""]+$";
                    break;
                    //case "RegularExpressionAttribute":
                    //    ramlTypeProp.Scalar.Pattern = "pattern: " + attribute.ConstructorArguments.First().Value;
                    //    break;
            }
            return new ScalarShape(prop.DataType, propPattern, propMinLength, propMaxLength, propMinimum, propMaximum, prop.ExclusiveMinimum, prop.ExclusiveMaximum,
                prop.Format, prop.MultipleOf, prop.Documentation, prop.XmlSerialization, prop.Examples, prop.Name, prop.DisplayName, prop.Description,
                prop.Default, prop.Values, prop.Inherits, prop.LinkTargetName);
        }

        private static decimal? ConvertToNullableDecimal(object value)
        {
            if (value == null)
                return null;

            return System.Convert.ToDecimal(value);
        }

    }
}
