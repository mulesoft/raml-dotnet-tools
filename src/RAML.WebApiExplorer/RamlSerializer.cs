using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer
{
	public class RamlSerializer
	{
		private const string RamlVersion = "0.8";
		public string Serialize(RamlDocument ramlDocument)
		{
			var sb = new StringBuilder(ramlDocument.Resources.Count + ramlDocument.Resources.Sum(r => r.Resources.Count) * 20);
			
			sb.AppendLine("#%RAML " + RamlVersion);

			SerializeProperty(sb, "title", ramlDocument.Title);

			SerializeProperty(sb, "baseUri", ramlDocument.BaseUri);

			SerializeProperty(sb, "version", ramlDocument.Version);

			SerializeProperty(sb, "mediaType", ramlDocument.MediaType);

			SerializeArrayProperty(sb, "securedBy", ramlDocument.SecuredBy);

			SerializeProtocols(sb, ramlDocument.Protocols);

			SerializeParameters(sb, "uriParameters", ramlDocument.BaseUriParameters);

			if (ramlDocument.Documentation.Any())
			{
				sb.AppendLine("documentation:");
				foreach (var docItem in ramlDocument.Documentation)
				{
					SerializeProperty(sb, "- title", docItem.Title, 2);
					SerializeMultilineProperty(sb, "content", docItem.Content, 4);
				}
				sb.AppendLine();
			}

			SerializeSecuritySchemes(sb, ramlDocument.SecuritySchemes);

		    SerializeSchemas(sb, ramlDocument.Schemas);

			SerializeResources(sb, ramlDocument.Resources);

			return sb.ToString();
		}

	    private void SerializeSchemas(StringBuilder sb, IEnumerable<IDictionary<string, string>> schemas)
	    {
            if(schemas == null || !schemas.Any() || schemas.All(x => !x.Any()))
                return;

	        sb.AppendLine("schemas:");
	        foreach (var kv in schemas.SelectMany(schemaDic => schemaDic))
	        {
	           SerializeSchema(sb, kv.Key, kv.Value, 2);
	        }
	    }

	    private void SerializeSecuritySchemes(StringBuilder sb, IEnumerable<IDictionary<string, SecurityScheme>> securitySchemes)
		{
			if (securitySchemes == null || !securitySchemes.Any()) return;

			sb.AppendLine("securitySchemes:");
			foreach (var scheme in securitySchemes)
			{
				SerializeSecurityScheme(sb, scheme, 2);
			}
		}

		private void SerializeSecurityScheme(StringBuilder sb, IDictionary<string, SecurityScheme> scheme, int indent)
		{
			foreach (var securityScheme in scheme)
			{
				sb.AppendLine(("- " + securityScheme.Key + ":").Indent(indent));
				SerializeDescriptionProperty(sb, securityScheme.Value.Description, indent + 4);
				if (securityScheme.Value.Type != null && securityScheme.Value.Type.Any())
					SerializeProperty(sb, "type", securityScheme.Value.Type.First().Key, indent + 4);

				SerializeSecurityDescriptor(sb, securityScheme.Value.DescribedBy, indent + 4);
				SerializeSecuritySettings(sb, securityScheme.Value.Settings, indent + 4);
			}
		}

        private void SerializeDescriptionProperty(StringBuilder sb, string description, int indentation)
	    {
            if (string.IsNullOrWhiteSpace(description))
                return;

            if (description.Contains(Environment.NewLine) || description.Contains("\r\n") || description.Contains("\n") || description.Contains("\r"))
            {
                SerializeMultilineProperty(sb, "description", description, indentation);
                return;
            }

            sb.AppendFormat("{0}: {1}".Indent(indentation), "description", "\"" + description.Replace("\"", string.Empty) + "\"");
            sb.AppendLine();
	    }

	    private void SerializeSecuritySettings(StringBuilder sb, SecuritySettings settings, int indent)
		{
			if(settings == null)
				return;

			sb.AppendLine("settings:".Indent(indent));
			SerializeProperty(sb, "accessTokenUri", settings.AccessTokenUri, indent + 2);
			SerializeProperty(sb, "authorizationUri", settings.AuthorizationUri, indent + 2);
			SerializeProperty(sb, "requestTokenUri", settings.RequestTokenUri, indent + 2);
			SerializeProperty(sb, "tokenCredentialsUri", settings.TokenCredentialsUri, indent + 2);
			SerializeArrayProperty(sb, "authorizationGrants", settings.AuthorizationGrants, indent + 2);
			SerializeListProperty(sb, "scopes", settings.Scopes, indent + 2);
		}

		private void SerializeListProperty(StringBuilder sb, string title, IEnumerable<string> enumerable, int indent)
		{
			sb.AppendLine((title + ":").Indent(indent));
			foreach (var value in enumerable)
			{
				sb.AppendLine(("- " + value).Indent(indent + 2));
			}
		}

		private void SerializeSecurityDescriptor(StringBuilder sb, SecuritySchemeDescriptor describedBy, int indent)
		{
			if(describedBy == null)
				return;

			sb.AppendLine("describedBy:".Indent(indent));
			SerializeParameters(sb, "headers", describedBy.Headers, indent + 2);
			SerializeParameters(sb, "queryParameters", describedBy.QueryParameters, indent + 2);

			if (describedBy.Responses != null && describedBy.Responses.Any())
			{
				sb.AppendLine("responses:".Indent(indent + 2));
				foreach (var response in describedBy.Responses)
				{
					response.Value.Code = response.Key;
					SerializeResponse(sb, response.Value, indent + 4);
				}
			}
		}

		private static void SerializeProtocols(StringBuilder sb, IEnumerable<Protocol> protocols, int indentation = 0)
		{
			if (protocols == null || !protocols.Any()) 
				return;

			sb.AppendFormat("protocols: {0}".Indent(indentation), "[" + string.Join(",", protocols) + "]");
			sb.AppendLine();
		}

		private void SerializeResources(StringBuilder sb, IEnumerable<Resource> resources, int indentation = 0)
		{
			foreach (var resource in resources)
			{
				SerializeResource(sb, resource, indentation);
			}
		}

		private void SerializeResource(StringBuilder sb, Resource resource, int indentation)
		{
			sb.AppendLine((resource.RelativeUri + ":").Indent(indentation));
			SerializeParameters(sb, "baseUriParameters", resource.BaseUriParameters, indentation + 2);
            SerializeDescriptionProperty(sb, resource.Description, indentation + 2);
			SerializeProperty(sb, "displayName", resource.DisplayName, indentation + 2);
			SerializeProtocols(sb, resource.Protocols, indentation + 2);
			SerializeParameters(sb, "uriParameters", resource.UriParameters, indentation + 2);
			SerializeMethods(sb, resource.Methods, indentation + 2);
			//SerializeType(sb, resource.Type, indentation + 2);
			SerializeResources(sb, resource.Resources, indentation + 2);
		}



		private void SerializeMethods(StringBuilder sb, IEnumerable<Method> methods, int indentation)
		{
			foreach (var method in methods)
			{
				SerializeMethod(sb, method, indentation);
			}
		}

		private void SerializeMethod(StringBuilder sb, Method method, int indentation)
		{
			sb.AppendLine((method.Verb + ":").Indent(indentation));
            SerializeDescriptionProperty(sb, method.Description, indentation + 2);
			//SerializeType(sb, method.Type, indentation + 2);
			
			if (method.Headers != null)
			{
				sb.AppendLine("headers:".Indent(indentation + 2));
				foreach (var header in method.Headers)
				{
					SerializeParameterProperties(sb, header, indentation + 4);
				}
			}

			SerializeArrayProperty(sb, "is", method.Is, indentation + 2);
			SerializeProtocols(sb, method.Protocols, indentation + 2);
			SerializeArrayProperty(sb, "securedBy", method.SecuredBy, indentation + 2);
			SerializeParameters(sb, "baseUriParameters", method.BaseUriParameters, indentation + 2);
			SerializeParameters(sb, "queryParameters", method.QueryParameters, indentation + 2);
			SerializeBody(sb, method.Body, indentation + 2);
			SerializeResponses(sb, method.Responses, indentation + 2);
			
		}

		private void SerializeBody(StringBuilder sb, IDictionary<string, MimeType> body, int indentation)
		{
			if(body == null || !body.Any())
				return;

			sb.AppendLine("body:".Indent(indentation));
			foreach (var mimeType in body)
			{
				SerializeMimeType(sb, mimeType, indentation + 2);
			}
		}

		private void SerializeMimeType(StringBuilder sb, KeyValuePair<string, MimeType> mimeType, int indentation)
		{
			sb.AppendLine((mimeType.Key + ":").Indent(indentation));
            SerializeDescriptionProperty(sb, mimeType.Value.Description, indentation + 2);
			SerializeProperty(sb, "type", mimeType.Value.Type, indentation + 2);
			SerializeParameters(sb, "formParameters", mimeType.Value.FormParameters, indentation + 2);
			SerializeProperty(sb, "schema", mimeType.Value.Schema, indentation + 2);
			SerializeProperty(sb, "example", mimeType.Value.Example, indentation + 2);
		}

		private void SerializeResponses(StringBuilder sb, IEnumerable<Response> responses, int indentation)
		{
			if(!responses.Any())
				return;

			sb.AppendLine("responses:".Indent(indentation));
			foreach (var response in responses)
			{
				SerializeResponse(sb, response, indentation + 2);
			}
		}

		private void SerializeResponse(StringBuilder sb, Response response, int indentation)
		{
			sb.AppendLine(response.Code.Indent(indentation) + ":");
            SerializeDescriptionProperty(sb, response.Description, indentation + 2);
			SerializeBody(sb, response.Body, indentation + 2);
		}

		private static void SerializeArrayProperty(StringBuilder sb, string enumerableTitle, IEnumerable<string> enumerable, int indentation = 0)
		{
			if(enumerable == null || !enumerable.Any())
				return;

			sb.AppendFormat((enumerableTitle + ": {0}").Indent(indentation), "[" + string.Join(",", enumerable) + "]");
			sb.AppendLine();
		}

		private static void SerializeMultilineProperty(StringBuilder sb, string propertyTitle, string propertyValue, int indentation)
		{
			sb.AppendFormat("{0}: |".Indent(indentation), propertyTitle);
			sb.AppendLine();
			var lines = propertyValue.Split(new []{ Environment.NewLine, "\r\n", "\n", "\r" }, StringSplitOptions.None);
			foreach (var line in lines)
			{
				sb.AppendLine(line.Indent(indentation + 2));
			}
		}

        private static void SerializeSchema(StringBuilder sb, string propertyTitle, string propertyValue, int indentation)
        {
            sb.AppendFormat("- {0}: |".Indent(indentation), propertyTitle);
            sb.AppendLine();
            var lines = propertyValue.Split(new[] { Environment.NewLine, "\r\n", "\n", "\r" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                sb.AppendLine(line.Indent(indentation + 4));
            }
        }


		private static void SerializeProperty(StringBuilder sb, string propertyTitle, string propertyValue, int indentation = 0)
		{
			if (string.IsNullOrWhiteSpace(propertyValue)) 
				return;

			if (propertyValue.Contains(Environment.NewLine) || propertyValue.Contains("\r\n") || propertyValue.Contains("\n") || propertyValue.Contains("\r"))
			{
				SerializeMultilineProperty(sb, propertyTitle, propertyValue, indentation);
				return;
			}

			sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
			sb.AppendLine();
		}

		private void SerializeParameters(StringBuilder sb, string parametersTitle, IDictionary<string, Parameter> parameters, int indentation = 0)
		{
			if (parameters == null || !parameters.Any())
				return;

			sb.AppendLine((parametersTitle + ":").Indent(indentation));
			foreach (var parameter in parameters)
			{
				SerializeParameter(sb, parameter, indentation + 2);
			}
		}

		private void SerializeParameter(StringBuilder sb, KeyValuePair<string, Parameter> parameter, int indentation)
		{
			sb.AppendFormat("{0}:".Indent(indentation), parameter.Key);
			sb.AppendLine();

			SerializeParameterProperties(sb, parameter.Value, indentation);
		}

		private void SerializeParameterProperties(StringBuilder sb, Parameter parameter, int indentation)
		{
			SerializeParameterProperty(sb, "default", parameter.Default, indentation + 2);
            SerializeDescriptionProperty(sb, parameter.Description, indentation + 2);
			SerializeParameterProperty(sb, "displayName", parameter.DisplayName, indentation + 2);
			SerializeParameterProperty(sb, "example", parameter.Example, indentation + 2);
			SerializeParameterProperty(sb, "pattern", parameter.Pattern, indentation + 2);
			SerializeParameterProperty(sb, "type", parameter.Type, indentation + 2);
			SerializeEnumProperty(sb, parameter.Enum, indentation + 2);
			SerializeParameterProperty(sb, "maxLength", parameter.MaxLength, indentation + 2);
			SerializeParameterProperty(sb, "maximum", parameter.Maximum, indentation + 2);
			SerializeParameterProperty(sb, "minimum", parameter.Minimum, indentation + 2);
			if(parameter.Repeat)
				SerializeParameterProperty(sb, "repeat", parameter.Repeat, indentation + 2);

			SerializeParameterProperty(sb, "required", parameter.Required, indentation + 2);
		}

		private void SerializeEnumProperty(StringBuilder sb, IEnumerable<string> enumerableProperty, int indentation)
		{
			if (enumerableProperty == null || !enumerableProperty.Any()) 
				return;

			sb.AppendFormat("enum: {0}".Indent(indentation), "[" + string.Join(",", enumerableProperty) + "]");
			sb.AppendLine();
		}

		private void SerializeParameterProperty(StringBuilder sb, string propertyTitle, int? propertyValue, int indentation)
		{
			if (propertyValue == null)
				return;

			sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
			sb.AppendLine();
		}

		private void SerializeParameterProperty(StringBuilder sb, string propertyTitle, decimal? propertyValue, int indentation)
		{
			if (propertyValue == null)
				return;

			sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
			sb.AppendLine();
		}

		private void SerializeParameterProperty(StringBuilder sb, string propertyTitle, bool? propertyValue, int indentation)
		{
			if (propertyValue == null)
				return;

			sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue.Value ? "true" : "false");
			sb.AppendLine();
		}

		private static void SerializeParameterProperty(StringBuilder sb, string propertyTitle, string propertyValue, int indentation)
		{
			if (string.IsNullOrWhiteSpace(propertyValue)) 
				return;

			sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
			sb.AppendLine();
		}
	}
}