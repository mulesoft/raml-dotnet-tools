using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace RAML.WebApiExplorer
{
	public class SchemaBuilder
	{
        private readonly IDictionary<string, Type> definitions = new Dictionary<string, Type>();
        private readonly ICollection<Type> types = new Collection<Type>();


		public string Get(Type type)
		{
            definitions.Clear();
            types.Clear();

		    string schema;

			if (type.IsGenericType && IsGenericWebResult(type))
				type = type.GetGenericArguments()[0];

		    if (IsArrayOrEnumerable(type))
		    {
		        var elementType = type.GetElementType() ?? type.GetGenericArguments()[0];
		        if (elementType.GetProperties().Count(p => p.CanWrite && p.SetMethod.IsPublic) == 0)
		            return null;

		        schema = GetMainArraySchema(elementType);
		    }
		    else
		    {
		        if (type.GetProperties().Count(p => p.CanWrite && p.SetMethod.IsPublic) == 0)
		            return null;

		        schema = GetMainObjectSchema(type);
		    }

		    schema = AddDefinitionsIfAny(schema);

            schema += "}" + Environment.NewLine;

		    return schema;
		}

	    private string AddDefinitionsIfAny(string schema)
	    {
	        if (definitions.Any())
	        {
	            schema = AddTrailingComma(schema);
	            schema += GetDefinitions();
	        }
	        return schema;
	    }

	    private static string AddTrailingComma(string schema)
	    {
            if(!String.IsNullOrWhiteSpace(schema) && schema.Length > "\r\n".Length)
                return schema.Substring(0, schema.Length - "\r\n".Length) + "," + Environment.NewLine;

	        return schema;
	    }

	    private string GetDefinitions()
	    {
	        var schema = "  \"definitions\": {" + Environment.NewLine;
	        foreach (var definition in definitions)
	        {
	            schema += "    \"" + definition.Key + "\": {" + Environment.NewLine;
                schema += "      \"properties\": {" + Environment.NewLine;
	            schema += GetProperties(definition.Value, 8);
	            schema += "      }";
                
                if(definition.Key == definitions.Last().Key)
                    schema += "    }" + Environment.NewLine;
                else
                    schema += "    }," + Environment.NewLine;
	        }
	        schema = RemoveTrailingComma(schema);
            schema += "  }" + Environment.NewLine;
	        return schema;
	    }

	    private static string RemoveTrailingComma(string schema)
	    {
	        schema = schema.Substring(0, schema.Length - 2) + Environment.NewLine;
	        return schema;
	    }

	    private string GetMainObjectSchema(Type type)
	    {
	        var objectSchema = "{ \r\n" +
	                           "  \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
	                           "  \"type\": \"object\",\r\n" +
                               "  \"id\": \"" + type.Name + "\",\r\n" +
	                           "  \"properties\": {\r\n";

            types.Add(type);

	        objectSchema += GetProperties(type, 4);

	        objectSchema += "  }\r\n";

	        return objectSchema;
	    }

	    private string GetMainArraySchema(Type elementType)
	    {
	        var arraySchema = "{\r\n" +
	                          "  \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
	                          "  \"type\": \"array\",\r\n" +
	                          "  \"items\": \r\n" +
	                          "  {\r\n" +
	                          "    \"type\": \"object\",\r\n" +
                              "    \"id\": \"" + elementType.Name + "\",\r\n" +
	                          "    \"properties\": \r\n" +
	                          "    {\r\n";
            types.Add(elementType);

	        arraySchema += GetProperties(elementType, 6);

	        arraySchema += "    }\r\n";
	        arraySchema += "  }\r\n";
	        return arraySchema;
	    }

	    private static bool IsGenericWebResult(Type type)
		{
			return (type.GetGenericTypeDefinition() == typeof (System.Web.Http.Results.OkNegotiatedContentResult<>)
					|| type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.NegotiatedContentResult<>)
					|| type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.CreatedAtRouteNegotiatedContentResult<>)
					|| type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.CreatedNegotiatedContentResult<>)
					|| type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.FormattedContentResult<>)
					|| type.GetGenericTypeDefinition() == typeof(System.Web.Http.Results.JsonResult<>));
		}

		private string GetRecursively(Type type, int pad)
		{
			if (type.IsGenericType && IsGenericWebResult(type))
				type = type.GetGenericArguments()[0];

			if (IsArrayOrEnumerable(type))
			{
				var elementType = type.GetElementType() ?? type.GetGenericArguments()[0];

			    if (SchemaTypeMapper.Map(elementType) != null)
			        return GetNestedArraySchemaPrimitiveType(pad, elementType);

				if (elementType.GetProperties().Count(p => p.CanWrite && p.SetMethod.IsPublic) == 0)
					return string.Empty;

			    return GetNestedArraySchema(pad, elementType);
			}
		    
            if (type.GetProperties().Count(p => p.CanWrite && p.SetMethod.IsPublic) == 0)
		        return string.Empty;

		    return GetNestedObjectSchema(type, pad);
		}

	    private string GetNestedObjectSchema(Type type, int pad)
	    {
	        if (types.Contains(type))
	        {
                return ("{ \"$ref\": \"" + type.Name + "\" }").Indent(pad) + Environment.NewLine;
	        }

            types.Add(type);
	        var objectSchema = "{ \r\n".Indent(pad) +
	                           "  \"type\": \"object\",\r\n".Indent(pad) +
                               ("  \"id\": \"" + type.Name + "\",\r\n").Indent(pad) + 
                               "  \"properties\": {\r\n".Indent(pad);

	        objectSchema += GetProperties(type, pad + 4);

	        objectSchema += "  }\r\n".Indent(pad);
	        objectSchema += "}\r\n".Indent(pad);
	        return objectSchema;
	    }

	    private string GetNestedArraySchema(int pad, Type elementType)
	    {
	        var arraySchema = "{\r\n".Indent(pad) +
	                          "  \"type\": \"array\",\r\n".Indent(pad) +
	                          "  \"items\": \r\n".Indent(pad);

	        if (types.Contains(elementType))
	        {
                arraySchema += ("{ \"$ref\": \"" + elementType.Name + "\" }").Indent(pad + 4) + Environment.NewLine;
	        }
	        else
	        {
                types.Add(elementType);
                arraySchema += "  {\r\n".Indent(pad) +
                               "    \"type\": \"object\",\r\n".Indent(pad) +
                               "    \"properties\": \r\n".Indent(pad) +
                               "    {\r\n".Indent(pad);

                arraySchema += GetProperties(elementType, pad + 4);

                arraySchema += "    }\r\n".Indent(pad);
                arraySchema += "  }\r\n".Indent(pad);            
	        }

	        arraySchema += "}\r\n".Indent(pad);
	        return arraySchema;
	    }

        private string GetNestedArraySchemaPrimitiveType(int pad, Type elementType)
        {
            var arraySchema = "{\r\n".Indent(pad) +
                              "  \"type\": \"array\",\r\n".Indent(pad) +
                              "  \"items\": \r\n".Indent(pad) +
                              "  {\r\n".Indent(pad) +
                              ("    \"type\": " + SchemaTypeMapper.GetAttribute(elementType) + "\r\n").Indent(pad) +
                              "  }\r\n".Indent(pad) +
                              "}\r\n".Indent(pad);
            return arraySchema;
        }

	    private string GetProperties(Type elementType, int pad)
		{
			var schema = string.Empty;
			var props = elementType.GetProperties().Where(p => p.CanWrite && p.SetMethod.IsPublic).ToArray();
			foreach (var prop in props)
			{
				schema = GetProperty(pad, prop, schema, props);
			}
			return schema;
		}

	    private string GetProperty(int pad, PropertyInfo prop, string schema, IEnumerable<PropertyInfo> props)
	    {
	        if (prop.PropertyType.IsEnum)
	            schema = HandleEnumProperty(pad, prop, props, schema);
	        else if (SchemaTypeMapper.Map(prop.PropertyType) != null)
	            schema = HandlePrimitiveTypeProperty(pad, prop, props, schema);
	        else 
                schema = HandleNestedTypeProperty(pad, prop, schema, props);
	        
            return schema;
	    }

	    private string HandleEnumProperty(int pad, PropertyInfo prop, IEnumerable<PropertyInfo> props, string schema)
	    {
            if (prop == props.Last())
                schema += GetEnumProperty(prop, pad) + Environment.NewLine;
            else
                schema += GetEnumProperty(prop, pad) + "," + Environment.NewLine;
            return schema;
	    }

	    private static string GetEnumProperty(PropertyInfo prop, int pad)
	    {
            return ("\"" + GetPropertyName(prop) + "\": { ").Indent(pad) + Environment.NewLine
                + ("  \"enum\": [" + string.Join(", ", Enum.GetNames(prop.PropertyType).Select(v => "\"" + v + "\"")) + "]").Indent(pad) + Environment.NewLine
                + "}".Indent(pad);
	    }

	    private static string HandlePrimitiveTypeProperty(int pad, PropertyInfo prop, IEnumerable<PropertyInfo> props, string schema)
	    {
	        if (prop == props.Last())
	            schema += BuildLastProperty(prop, pad);
	        else
	            schema += BuildProperty(prop, pad);
	        return schema;
	    }

	    private string HandleNestedTypeProperty(int pad, PropertyInfo prop, string schema, IEnumerable<PropertyInfo> props)
	    {
	        var nestedType = GetRecursively(prop.PropertyType, pad + 2);
            if(nestedType == null)
                return string.Empty;

	        if (!string.IsNullOrWhiteSpace(nestedType))
	        {
                var name = GetPropertyName(prop);
	            schema += ("\"" + name + "\":").Indent(pad) + Environment.NewLine;
	            schema += nestedType;
	        }
	        else
	        {
	            // is it one of ?
	            var subclasses =prop.PropertyType.Assembly.GetTypes()
                    .Where(type => type.IsSubclassOf(prop.PropertyType))
                    .ToArray();
	            if (subclasses.Any())
	                schema += GetOneOfProperty(prop, subclasses, pad);
	        }

            if (prop != props.Last() && !String.IsNullOrWhiteSpace(schema) && schema.Length > "\r\n".Length)
	            schema = schema.Substring(0, schema.Length - "\r\n".Length) + ",\r\n";

	        return schema;
	    }

	    private string GetOneOfProperty(PropertyInfo prop, IEnumerable<Type> subclasses, int pad)
	    {
            var name = GetPropertyName(prop);
	        var oneOf = ("\"" + name + "\": {").Indent(pad) + Environment.NewLine;
	        oneOf += "\"type\": \"object\",".Indent(pad + 2) + Environment.NewLine;
	        oneOf += "\"oneOf\": [".Indent(pad + 2) + Environment.NewLine;
	        foreach (var subclass in subclasses.Distinct())
	        {
	            var className = GetClassName(subclass);
	            oneOf += ("{ \"$ref\": \"#/definitions/" + className + "\" },").Indent(pad + 4) + Environment.NewLine;
                if (!definitions.ContainsKey(className)) { 
                    definitions.Add(className, subclass);
}
	        }
	        oneOf = oneOf.Substring(0, oneOf.Length - Environment.NewLine.Length - 1) + Environment.NewLine;
	        oneOf += "]".Indent(pad + 2) + Environment.NewLine;
	        oneOf += "}".Indent(pad) + Environment.NewLine;
	        return oneOf;
	    }

	    private static string BuildProperty(PropertyInfo prop, int pad)
		{
	        var name = GetPropertyName(prop);

            var res = "\"" + name + "\": { \"type\": " + SchemaTypeMapper.GetAttribute(prop.PropertyType);
			
			if (IsNullable(prop.PropertyType))
				res += ", \"required\": \"false\"";

			res += "},\r\n";

			res = res.Indent(pad);
			return res;
		}

	    private static string GetPropertyName(MemberInfo property)
	    {
            return GetMemberNameByType(property, typeof(JsonPropertyAttribute));
	    }

        private static string GetClassName(MemberInfo @class)
        {
            return GetMemberNameByType(@class, typeof(JsonObjectAttribute));
        }

        private static string GetMemberNameByType(MemberInfo @class, Type attributeType)
        {
            var className = @class.Name;
            if (@class.CustomAttributes.All(a => a.AttributeType != attributeType))
                return className;

            var attr = @class.CustomAttributes.First(a => a.AttributeType == attributeType);
            if (attr.ConstructorArguments.Any())
                className = attr.ConstructorArguments.First().Value.ToString();

            return className;
        }


	    private static string BuildLastProperty(PropertyInfo prop, int pad)
		{
            var name = GetPropertyName(prop);
            var res = "\"" + name + "\": { \"type\": " + SchemaTypeMapper.GetAttribute(prop.PropertyType);

			if (IsNullable(prop.PropertyType))
				res += ", \"required\": \"false\"";

			res += "}\r\n";

			res = res.Indent(pad);
			return res;
		}

		public static bool IsNullable(Type t)
		{
			return Nullable.GetUnderlyingType(t) != null;
		}

	    private static bool IsArrayOrEnumerable(Type type)
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