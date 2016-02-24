using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace RAML.WebApiExplorer
{
    public abstract class TypeBuilderBase
    {
        protected readonly ICollection<Type> Types = new Collection<Type>();

        protected static bool IsGenericWebResult(Type type)
        {
            return (type.GetGenericTypeDefinition() == typeof (System.Web.Http.Results.OkNegotiatedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.NegotiatedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.CreatedAtRouteNegotiatedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.CreatedNegotiatedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.FormattedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.JsonResult<>));
        }

        protected string GetProperties(Type elementType, int pad)
        {
            var schema = string.Empty;
            var props = elementType.GetProperties().Where(p => p.CanRead || p.CanWrite).ToArray();
            foreach (var prop in props)
            {
                schema = GetProperty(pad, prop, schema, props, prop.CustomAttributes);
            }
            return schema;
        }

        protected string GetProperty(int pad, PropertyInfo prop, string schema, IEnumerable<PropertyInfo> props, IEnumerable<CustomAttributeData> customAttributes)
        {
            if (prop.PropertyType.IsEnum)
                schema = HandleEnumProperty(pad, prop, props, schema);
            else if (SchemaTypeMapper.Map(prop.PropertyType) != null)
                schema = HandlePrimitiveTypeProperty(pad, prop, props, schema, customAttributes);
            else 
                schema = HandleNestedTypeProperty(pad, prop, schema, props, customAttributes);
	        
            return schema;
        }

        protected abstract string HandleEnumProperty(int pad, PropertyInfo prop, IEnumerable<PropertyInfo> props, string schema);

        protected abstract string HandlePrimitiveTypeProperty(int pad, PropertyInfo prop, IEnumerable<PropertyInfo> props, string schema, IEnumerable<CustomAttributeData> customAttributes);

        protected abstract string HandleNestedTypeProperty(int pad, PropertyInfo prop, string schema, IEnumerable<PropertyInfo> props, IEnumerable<CustomAttributeData> customAttributes);


        protected string HandleValidationAttributes(IEnumerable<CustomAttributeData> customAttributes)
        {
            return customAttributes.Where(p => p.AttributeType != typeof (RequiredAttribute)).Aggregate(string.Empty, (current, p) => current + HandleValidationAttribute(p));
        }

        protected abstract string HandleValidationAttribute(CustomAttributeData customAttributeData);


        protected static bool IsMaxValue(CustomAttributeTypedArgument argument)
        {
            if (argument.ArgumentType == typeof (int))
                return int.MaxValue == (int) argument.Value;

            return Math.Abs(double.MaxValue - (double) argument.Value) < 1;
        }

        protected static bool IsMinValue(CustomAttributeTypedArgument argument)
        {
            if (argument.ArgumentType == typeof(int))
                return int.MinValue == (int)argument.Value;

            return Math.Abs(double.MinValue - (double)argument.Value) < 1;
        }

        protected static object Format(CustomAttributeTypedArgument argument)
        {
            var us = new CultureInfo("en-US");
            return argument.ArgumentType == typeof (int)
                ? argument.Value
                : Convert.ToDouble(argument.Value).ToString("F", us);
        }

        protected static string GetPropertyName(MemberInfo property)
        {
            return GetMemberNameByType(property, typeof(JsonPropertyAttribute));
        }

        protected static string GetClassName(MemberInfo @class)
        {
            return GetMemberNameByType(@class, typeof(JsonObjectAttribute));
        }

        protected static string GetMemberNameByType(MemberInfo @class, Type attributeType)
        {
            var className = @class.Name;
            if (@class.CustomAttributes.All(a => a.AttributeType != attributeType))
                return className;

            var attr = @class.CustomAttributes.First(a => a.AttributeType == attributeType);
            if (attr.ConstructorArguments.Any())
                className = attr.ConstructorArguments.First().Value.ToString();

            return className;
        }

        public static bool IsNullable(Type t)
        {
            return Nullable.GetUnderlyingType(t) != null;
        }

        protected static bool IsArrayOrEnumerable(Type type)
        {
            return type.IsArray || (type.IsGenericType &&
                                    (type.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                                     || type.GetGenericTypeDefinition() == typeof (ICollection<>)
                                     || type.GetGenericTypeDefinition() == typeof (Collection<>)
                                     || type.GetGenericTypeDefinition() == typeof (IList<>)
                                     || type.GetGenericTypeDefinition() == typeof (List<>)
                                        )
                );
        }

    }
}