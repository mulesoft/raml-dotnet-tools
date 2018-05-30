using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raml.Parser.Expressions;

namespace AMF.WebApiExplorer
{
	public class RamlSerializer
	{
        private readonly Raml1TypesSerializer raml1TypesSerializer = new Raml1TypesSerializer();

		public string Serialize(RamlDocument ramlDocument)
		{
			var sb = new StringBuilder(ramlDocument.Resources.Count + ramlDocument.Resources.Sum(r => r.Resources.Count) * 20);

            sb.AppendLine("#%RAML " + (ramlDocument.RamlVersion == RamlVersion.Version08 ? "0.8" : "1.0"));

            RamlSerializerHelper.SerializeProperty(sb, "title", ramlDocument.Title);

            RamlSerializerHelper.SerializeProperty(sb, "baseUri", ramlDocument.BaseUri);

            RamlSerializerHelper.SerializeProperty(sb, "version", ramlDocument.Version);

            RamlSerializerHelper.SerializeProperty(sb, "mediaType", ramlDocument.MediaType);

			SerializeArrayProperty(sb, "securedBy", ramlDocument.SecuredBy);

			SerializeProtocols(sb, ramlDocument.Protocols);

			SerializeParameters(sb, "uriParameters", ramlDocument.BaseUriParameters);

			if (ramlDocument.Documentation.Any())
			{
				sb.AppendLine("documentation:");
				foreach (var docItem in ramlDocument.Documentation)
				{
                    RamlSerializerHelper.SerializeProperty(sb, "- title", docItem.Title, 2);
                    RamlSerializerHelper.SerializeMultilineProperty(sb, "content", docItem.Content, 4);
                    raml1TypesSerializer.SerializeAnnotations(sb, docItem.Annotations, 4);
				}
				sb.AppendLine();
			}

			SerializeSecuritySchemes(sb, ramlDocument.SecuritySchemes);

            raml1TypesSerializer.Serialize(sb, ramlDocument.Types);

		    SerializeSchemas(sb, ramlDocument.Schemas);

			SerializeResources(sb, ramlDocument.Resources);

            SerializeAnnotationTypes(sb, ramlDocument.AnnotationTypes);

            raml1TypesSerializer.SerializeAnnotations(sb, ramlDocument.Annotations);

			return sb.ToString();
		}

	    private void SerializeAnnotationTypes(StringBuilder sb, IDictionary<string, AnnotationType> annotationTypes)
	    {
            if(annotationTypes == null || !annotationTypes.Any())
                return;

            sb.Append("annotations:");
	        sb.AppendLine();

	        foreach (var annotationType in annotationTypes)
	        {
	            sb.AppendFormat("{0}:".Indent(4), annotationType.Key);
	            sb.AppendLine();
                SerializeAnnotationType(sb, annotationType.Value);
	        }
	    }

	    private void SerializeAnnotationType(StringBuilder sb, AnnotationType annotationType)
	    {
            RamlSerializerHelper.SerializeProperty(sb, "description", annotationType.Description, 8);
            RamlSerializerHelper.SerializeProperty(sb, "displayName", annotationType.DisplayName, 8);
            RamlSerializerHelper.SerializeProperty(sb, "allowMultiple", annotationType.AllowMultiple, 8);
            RamlSerializerHelper.SerializeProperty(sb, "usage", annotationType.Usage, 8);
            SerializeArrayProperty(sb, "allowedTargets", annotationType.AllowedTargets, 8);
            foreach (var parameter in annotationType.Parameters)
            {
                SerializeParameter(sb, parameter, 8);
            }
            raml1TypesSerializer.SerializeAnnotations(sb, annotationType.Annotations, 8);
	    }


	    private void SerializeSchemas(StringBuilder sb, IEnumerable<IDictionary<string, string>> schemas)
	    {
            if(schemas == null || !schemas.Any() || schemas.All(x => !x.Any()))
                return;

	        sb.AppendLine("schemas:");
	        foreach (var kv in schemas.SelectMany(schemaDic => schemaDic))
	        {
                RamlSerializerHelper.SerializeSchema(sb, kv.Key, kv.Value, 2);
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
                RamlSerializerHelper.SerializeDescriptionProperty(sb, securityScheme.Value.Description, indent + 4);
				if (securityScheme.Value.Type != null && securityScheme.Value.Type.Any())
                    RamlSerializerHelper.SerializeProperty(sb, "type", securityScheme.Value.Type.First().Key, indent + 4);

				SerializeSecurityDescriptor(sb, securityScheme.Value.DescribedBy, indent + 4);
				SerializeSecuritySettings(sb, securityScheme.Value.Settings, indent + 4);
			}
		}


	    private void SerializeSecuritySettings(StringBuilder sb, SecuritySettings settings, int indent)
		{
			if(settings == null)
				return;

			sb.AppendLine("settings:".Indent(indent));
            RamlSerializerHelper.SerializeProperty(sb, "accessTokenUri", settings.AccessTokenUri, indent + 2);
            RamlSerializerHelper.SerializeProperty(sb, "authorizationUri", settings.AuthorizationUri, indent + 2);
            RamlSerializerHelper.SerializeProperty(sb, "requestTokenUri", settings.RequestTokenUri, indent + 2);
            RamlSerializerHelper.SerializeProperty(sb, "tokenCredentialsUri", settings.TokenCredentialsUri, indent + 2);
			SerializeArrayProperty(sb, "authorizationGrants", settings.AuthorizationGrants, indent + 2);
            RamlSerializerHelper.SerializeListProperty(sb, "scopes", settings.Scopes, indent + 2);
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
					response.Value.Code = Convert.ToInt32(response.Key);
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
            RamlSerializerHelper.SerializeDescriptionProperty(sb, resource.Description, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "displayName", resource.DisplayName, indentation + 2);
			SerializeProtocols(sb, resource.Protocols, indentation + 2);
			SerializeParameters(sb, "uriParameters", resource.UriParameters, indentation + 2);
			SerializeMethods(sb, resource.Methods, indentation + 2);
			//SerializeType(sb, resource.Type, indentation + 2);
			SerializeResources(sb, resource.Resources, indentation + 2);
            raml1TypesSerializer.SerializeAnnotations(sb, resource.Annotations, indentation + 2);
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
            RamlSerializerHelper.SerializeDescriptionProperty(sb, method.Description, indentation + 2);
			//SerializeType(sb, method.Type, indentation + 2);
			
			if (method.Headers != null)
			{
				sb.AppendLine("headers:".Indent(indentation + 2));
				foreach (var header in method.Headers)
				{
                    sb.AppendLine(header.Key + ":".Indent(indentation + 4));
					RamlSerializerHelper.SerializeParameterProperties(sb, header.Value, indentation + 6);
				}
			}

			SerializeArrayProperty(sb, "is", method.Is, indentation + 2);
			SerializeProtocols(sb, method.Protocols, indentation + 2);
			SerializeArrayProperty(sb, "securedBy", method.SecuredBy, indentation + 2);
			SerializeParameters(sb, "baseUriParameters", method.BaseUriParameters, indentation + 2);
			SerializeParameters(sb, "queryParameters", method.QueryParameters, indentation + 2);
			SerializeBody(sb, method.Body, indentation + 2);
			SerializeResponses(sb, method.Responses, indentation + 2);
            raml1TypesSerializer.SerializeAnnotations(sb, method.Annotations, indentation + 2);
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
            RamlSerializerHelper.SerializeDescriptionProperty(sb, mimeType.Value.Description, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "type", mimeType.Value.Type, indentation + 2);
			SerializeParameters(sb, "formParameters", mimeType.Value.FormParameters, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "schema", mimeType.Value.Schema, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "example", mimeType.Value.Example, indentation + 2);
            raml1TypesSerializer.SerializeAnnotations(sb, mimeType.Value.Annotations, indentation + 2);
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
			sb.AppendLine(response.Code.ToString().Indent(indentation) + ":");
            RamlSerializerHelper.SerializeDescriptionProperty(sb, response.Description, indentation + 2);
			SerializeBody(sb, response.Body, indentation + 2);
		}

		private static void SerializeArrayProperty(StringBuilder sb, string enumerableTitle, IEnumerable<string> enumerable, int indentation = 0)
		{
			if(enumerable == null || !enumerable.Any())
				return;

			sb.AppendFormat((enumerableTitle + ": {0}").Indent(indentation), "[" + string.Join(",", enumerable) + "]");
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

			RamlSerializerHelper.SerializeParameterProperties(sb, parameter.Value, indentation);
		}



	}
}