using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer
{
    public class Raml1TypeBuilder
    {
        private readonly RamlTypesOrderedDictionary raml1Types;
        private readonly ICollection<Type> types = new Collection<Type>();

        public Raml1TypeBuilder(RamlTypesOrderedDictionary raml1Types)
        {
            this.raml1Types = raml1Types;
        }

        public string Add(Type type)
        {
            var typeName = GetTypeName(type);

            if(types.Contains(type))
                return typeName;

            RamlType raml1Type;

            if (type.IsGenericType && TypeBuilderHelper.IsGenericWebResult(type))
                type = type.GetGenericArguments()[0];

            if (TypeBuilderHelper.IsArrayOrEnumerable(type))
            {
                var elementType = GetElementType(type);
                if (TypeBuilderHelper.IsArrayOrEnumerable(elementType))
                {
                    var subElementType = GetElementType(elementType);

                    if (Raml1TypeMapper.Map(subElementType) != null)
                        return Raml1TypeMapper.Map(subElementType) + "[][]";

                    if (!HasPropertiesOrParentType(subElementType))
                        return string.Empty;

                    types.Add(type);

                    raml1Type = GetArrayOfArray(subElementType);
                }
                else
                {
                    if (Raml1TypeMapper.Map(elementType) != null)
                        return Raml1TypeMapper.Map(elementType) + "[]";

                    if (!HasPropertiesOrParentType(elementType))
                        return string.Empty;

                    types.Add(type);

                    raml1Type = GetArray(elementType);                    
                }
            }
            else if (IsDictionary(type) || IsParentADictionary(type) )
            {
                types.Add(type);
                raml1Type = GetMap(type);
            }
            else if (type.IsEnum)
            {
                types.Add(type);
                raml1Type = GetEnum(type);
            }
            else if (Raml1TypeMapper.Map(type) != null)
            {
                //raml1Type = GetScalar(type);
                raml1Type = null;
                typeName = Raml1TypeMapper.Map(type);
            }
            else
            {
                if (!HasPropertiesOrParentType(type))
                    return string.Empty;

                types.Add(type);
                raml1Type = GetObject(type);
            }

            if(raml1Type != null)
                AddType(type, raml1Type);

            return typeName;
        }

        private static bool IsParentADictionary(Type type)
        {
            return (type.BaseType != null && type.BaseType != typeof(Object) && IsDictionary(type.BaseType));
        }

        private RamlType GetMap(Type type)
        {
            Type subtype;

            if(IsDictionary(type))
                subtype = type.GetGenericArguments()[1];
            else
                subtype = type.BaseType.GetGenericArguments()[1];


            var subtypeName = GetTypeName(subtype);

            if (Raml1TypeMapper.Map(subtype) != null)
                subtypeName = Raml1TypeMapper.Map(subtype);
            else
                subtypeName = Add(subtype);

            if(string.IsNullOrWhiteSpace(subtypeName))
                return null;

            var raml1Type = new RamlType
            {
                Object = new ObjectType
                {
                    Properties = new Dictionary<string, RamlType>()
                    {
                        {
                            "[]", new RamlType
                            {
                                Type = subtypeName,
                                Required = true
                            }
                        }
                    }
                }
            };

            return raml1Type;
        }

        private RamlType GetArrayOfArray(Type subElementType)
        {
            string elementTypeName;
            if (Raml1TypeMapper.Map(subElementType) == null)
                elementTypeName = Add(subElementType);
            else
                elementTypeName = Raml1TypeMapper.Map(subElementType);

            if (string.IsNullOrWhiteSpace(elementTypeName))
                return null;

            var raml1Type = new RamlType
            {
                Array = new ArrayType
                {
                    Items = new RamlType
                    {
                        Array = new ArrayType
                        {
                            Items = new RamlType
                            {
                                Name = GetTypeName(subElementType),
                                Type = elementTypeName
                            }
                        }
                    }
                }
            };

            return raml1Type;
        }

        private RamlType GetArray(Type elementType)
        {
            string elementTypeName;
            if (Raml1TypeMapper.Map(elementType) == null)
                elementTypeName = Add(elementType);
            else
                elementTypeName = Raml1TypeMapper.Map(elementType);
                
            
            if (string.IsNullOrWhiteSpace(elementTypeName))
                return null;

            return new RamlType
            {
                Array = new ArrayType
                {
                    Items = new RamlType
                    {
                        Name = GetTypeName(elementType),
                        Type = elementTypeName
                    }
                }
            };
        }

        private RamlType GetScalar(Type type)
        {
            var ramlType = new RamlType
            {
                Scalar = new Property
                {
                    Type = Raml1TypeMapper.Map(type),
                }
            };
            return ramlType;
        }

        private RamlType GetEnum(Type type)
        {
            var ramlType = new RamlType
            {
                Scalar = new Property { Enum = type.GetEnumNames() }
            };

            return ramlType;
        }

        private RamlType GetObject(Type type)
        {
            var raml1Type = new RamlType();
            raml1Type.Object = new ObjectType();
            raml1Type.Type = "object";

            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                var parent = GetObject(type.BaseType);
                AddType(type.BaseType, parent);
                raml1Type.Type = GetTypeName(type.BaseType);
            }

            if (GetClassProperties(type).Count(p => p.CanWrite) > 0)
            {
                raml1Type.Object.Properties = GetProperties(type);
            }
            return raml1Type;
        }

        private IDictionary<string, RamlType> GetProperties(Type type)
        {
            var props = GetClassProperties(type).ToArray();
            var dic = new Dictionary<string, RamlType>();
            foreach (var prop in props)
            {
                var key = TypeBuilderHelper.GetPropertyName(prop);
                var ramlType = GetProperty(prop);
                
                if (ramlType != null)
                {
                    ramlType.Required = !IsOptionalProperty(prop, prop.CustomAttributes);
                    dic.Add(key, ramlType);
                }
            }
            return dic;
        }

        private static IEnumerable<PropertyInfo> GetClassProperties(Type type)
        {
            var properties = type.GetProperties().Where(p => p.CanWrite);
            if (type.BaseType != null && type.BaseType != typeof (Object))
            {
                var parentProperties = type.BaseType.GetProperties().Where(p => p.CanWrite);
                properties = properties.Where(p => parentProperties.All(x => x.Name != p.Name));
            }
            return properties;
        }

        private RamlType GetProperty(PropertyInfo prop)
        {
            if (prop.PropertyType.IsEnum)
                return GetEnum(prop.PropertyType);
            
            if (Raml1TypeMapper.Map(prop.PropertyType) != null)
                return HandlePrimitiveTypeProperty(prop);

            if (TypeBuilderHelper.IsArrayOrEnumerable(prop.PropertyType))
                return GetArray(prop.PropertyType);
            
            if (IsDictionary(prop.PropertyType))
                return GetMap(prop.PropertyType);
            
            return HandleNestedTypeProperty(prop);
        }

        private RamlType HandlePrimitiveTypeProperty(PropertyInfo prop)
        {
            var ramlTypeProp = GetScalar(prop.PropertyType);
            ramlTypeProp.Name = prop.Name;
            HandleValidationAttributes(ramlTypeProp, prop.CustomAttributes);
            return ramlTypeProp;
        }

        private void HandleValidationAttributes(RamlType ramlTypeProp, IEnumerable<CustomAttributeData> customAttributes)
        {
            foreach (var attribute in customAttributes)
            {
                HandleValidationAttribute(ramlTypeProp, attribute);
            }
        }

        private static void HandleValidationAttribute(RamlType ramlTypeProp, CustomAttributeData attribute)
        {
            switch (attribute.AttributeType.Name)
            {
                case "MaxLengthAttribute":
                    ramlTypeProp.Scalar.MaxLength = (int?) attribute.ConstructorArguments.First().Value;
                    break;
                case "MinLengthAttribute":
                    ramlTypeProp.Scalar.MinLength = (int?) attribute.ConstructorArguments.First().Value;
                    break;
                case "RangeAttribute":
                    if (!TypeBuilderHelper.IsMinValue(attribute.ConstructorArguments.First()))
                        ramlTypeProp.Scalar.Minimum = ConvertToNullableDecimal(attribute.ConstructorArguments.First().Value);
                    if (!TypeBuilderHelper.IsMaxValue(attribute.ConstructorArguments.Last()))
                        ramlTypeProp.Scalar.Maximum = ConvertToNullableDecimal(attribute.ConstructorArguments.Last().Value);
                    break;
                case "EmailAddressAttribute":
                    ramlTypeProp.Scalar.Pattern = @"pattern: [^\\s@]+@[^\\s@]+\\.[^\\s@]";
                    break;
                case "UrlAttribute":
                    ramlTypeProp.Scalar.Pattern = @"pattern: ^(ftp|http|https):\/\/[^ \""]+$";
                    break;
                //case "RegularExpressionAttribute":
                //    ramlTypeProp.Scalar.Pattern = "pattern: " + attribute.ConstructorArguments.First().Value;
                //    break;
            }
        }

        private static decimal? ConvertToNullableDecimal(object value)
        {
            if (value == null)
                return null;

            return Convert.ToDecimal(value);
        }

        private RamlType HandleNestedTypeProperty(PropertyInfo prop)
        {
            var typeName = Add(prop.PropertyType);

            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            return new RamlType { Type = typeName };
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

        private void AddType(Type type, RamlType raml1Type)
        {
            var typeName = GetTypeName(type);

            // handle case of different types with same class name
            if (raml1Types.ContainsKey(typeName))
                typeName = GetUniqueName(typeName);

            raml1Types.Add(typeName, raml1Type);
        }

        private static string GetTypeName(Type type)
        {
            var typeName = type.Name;

            if (IsDictionary(type))
                typeName = type.GetGenericArguments()[1].Name + "Map";

            if (TypeBuilderHelper.IsArrayOrEnumerable(type))
                typeName = "ListOf" + GetTypeName(GetElementType(type));

            return typeName.Replace("`", string.Empty);
        }

        private string GetUniqueName(string schemaName)
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