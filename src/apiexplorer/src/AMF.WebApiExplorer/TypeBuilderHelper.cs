using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace AMF.WebApiExplorer
{
    public static class TypeBuilderHelper
    {
        

        public static bool IsGenericWebResult(Type type)
        {
            return (type.GetGenericTypeDefinition() == typeof (System.Web.Http.Results.OkNegotiatedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.NegotiatedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.CreatedAtRouteNegotiatedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.CreatedNegotiatedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.FormattedContentResult<>)
                    || type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.JsonResult<>));
        }

        public static bool IsMaxValue(CustomAttributeTypedArgument argument)
        {
            if (argument.ArgumentType == typeof (int))
                return int.MaxValue == (int) argument.Value;

            return Math.Abs(double.MaxValue - (double) argument.Value) < 1;
        }

        public static bool IsMinValue(CustomAttributeTypedArgument argument)
        {
            if (argument.ArgumentType == typeof(int))
                return int.MinValue == (int)argument.Value;

            return Math.Abs(double.MinValue - (double)argument.Value) < 1;
        }

        public static object Format(CustomAttributeTypedArgument argument)
        {
            var us = new CultureInfo("en-US");
            return argument.ArgumentType == typeof (int)
                ? argument.Value
                : Convert.ToDouble(argument.Value).ToString("F", us);
        }

        public static string GetPropertyName(MemberInfo property)
        {
            return GetMemberNameByType(property, typeof(JsonPropertyAttribute));
        }

        public static string GetClassName(MemberInfo @class)
        {
            return GetMemberNameByType(@class, typeof(JsonObjectAttribute));
        }

        public static string GetMemberNameByType(MemberInfo @class, Type attributeType)
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

        public static bool IsArrayOrEnumerable(Type type)
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