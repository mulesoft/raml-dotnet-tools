using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMF.Parser.Model;

namespace RAML.WebApiExplorer
{
	public class RamlSerializer
	{
        private readonly Raml1TypesSerializer raml1TypesSerializer = new Raml1TypesSerializer();

		public string Serialize(AmfModel amfModel, ApiExplorerService.RamlVersion ramlVersion)
		{
            StringBuilder sb = null;
            var shapesCount = (amfModel.Shapes != null ? amfModel.Shapes.Count() : 0) * 6;

            if (amfModel.WebApi == null)
            {
                sb = new StringBuilder(shapesCount);
                sb.AppendLine("#%RAML " + (ramlVersion == ApiExplorerService.RamlVersion.Version08 ? "0.8" : "1.0"));
                raml1TypesSerializer.Serialize(sb, amfModel.Shapes);
            }
            else
            {
                var ramlDocument = amfModel.WebApi;

                sb = new StringBuilder( (ramlDocument.EndPoints == null ? 0 : ramlDocument.EndPoints.Count()) * 20 + shapesCount);

                sb.AppendLine("#%RAML " + (ramlVersion == ApiExplorerService.RamlVersion.Version08 ? "0.8" : "1.0"));

                RamlSerializerHelper.SerializeProperty(sb, "title", ramlDocument.Name);

                RamlSerializerHelper.SerializeProperty(sb, "baseUri", GetUrl(ramlDocument));

                RamlSerializerHelper.SerializeProperty(sb, "version", ramlDocument.Version);

                RamlSerializerHelper.SerializeProperty(sb, "mediaType", ramlDocument.ContentType?.First());

                //TODO: check
                SerializeArrayProperty(sb, "securedBy", ramlDocument.Security?.Select(s => s.Name).ToArray());

                SerializeProtocols(sb, ramlDocument.Schemes);

                SerializeParameters(sb, "uriParameters", ramlDocument.BaseUriParameters);

                if (ramlDocument.Documentations != null && ramlDocument.Documentations.Any())
                {
                    sb.AppendLine("documentation:");
                    foreach (var docItem in ramlDocument.Documentations)
                    {
                        RamlSerializerHelper.SerializeProperty(sb, "- title", docItem.Title, 2);
                        RamlSerializerHelper.SerializeMultilineProperty(sb, "content", docItem.Description, 4);
                        // raml1TypesSerializer.SerializeAnnotations(sb, docItem.Annotations, 4);
                    }
                    sb.AppendLine();
                }

                SerializeSecuritySchemes(sb, ramlDocument.Security);

                raml1TypesSerializer.Serialize(sb, amfModel.Shapes);

                //SerializeSchemas(sb, ramlDocument.Schemas);

                SerializeEndPoints(sb, ramlDocument.EndPoints);

                //SerializeAnnotationTypes(sb, ramlDocument.AnnotationTypes);

                //raml1TypesSerializer.SerializeAnnotations(sb, ramlDocument.Annotations);
            }

			return sb.ToString();
		}

        private string GetUrl(WebApi ramlDocument)
        {
            var url = string.Empty;
            if(ramlDocument.Schemes != null && ramlDocument.Schemes.Any())
                url += ramlDocument.Schemes.First();

            if(!string.IsNullOrWhiteSpace(ramlDocument.Host))
                url += (string.IsNullOrWhiteSpace(url) ? "http://" : "://") + ramlDocument.Host;

            if (!string.IsNullOrWhiteSpace(ramlDocument.BasePath))
                url += ramlDocument.BasePath.StartsWith("/") ? ramlDocument.BasePath : "/" + ramlDocument.BasePath;

            return url;
        }

     //   private void SerializeAnnotationTypes(StringBuilder sb, IDictionary<string, AnnotationType> annotationTypes)
	    //{
     //       if(annotationTypes == null || !annotationTypes.Any())
     //           return;

     //       sb.Append("annotations:");
	    //    sb.AppendLine();

	    //    foreach (var annotationType in annotationTypes)
	    //    {
	    //        sb.AppendFormat("{0}:".Indent(4), annotationType.Key);
	    //        sb.AppendLine();
     //           SerializeAnnotationType(sb, annotationType.Value);
	    //    }
	    //}

	    //private void SerializeAnnotationType(StringBuilder sb, AnnotationType annotationType)
	    //{
     //       RamlSerializerHelper.SerializeProperty(sb, "description", annotationType.Description, 8);
     //       RamlSerializerHelper.SerializeProperty(sb, "displayName", annotationType.DisplayName, 8);
     //       RamlSerializerHelper.SerializeProperty(sb, "allowMultiple", annotationType.AllowMultiple, 8);
     //       RamlSerializerHelper.SerializeProperty(sb, "usage", annotationType.Usage, 8);
     //       SerializeArrayProperty(sb, "allowedTargets", annotationType.AllowedTargets, 8);
     //       foreach (var parameter in annotationType.Parameters)
     //       {
     //           SerializeParameter(sb, parameter, 8);
     //       }
     //       raml1TypesSerializer.SerializeAnnotations(sb, annotationType.Annotations, 8);
	    //}


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

	    private void SerializeSecuritySchemes(StringBuilder sb, IEnumerable<ParametrizedSecurityScheme> security)
		{
			if (security == null || !security.Any()) return;

			sb.AppendLine("securitySchemes:");
			foreach (var scheme in security)
			{
				SerializeSecurityScheme(sb, scheme, 2);
			}
		}

		private void SerializeSecurityScheme(StringBuilder sb, ParametrizedSecurityScheme securityScheme, int indent)
		{
			sb.AppendLine(("- " + securityScheme.Name + ":").Indent(indent));
            RamlSerializerHelper.SerializeDescriptionProperty(sb, securityScheme.SecurityScheme.Description, indent + 4);
			if (securityScheme.SecurityScheme.Type != null)
                RamlSerializerHelper.SerializeProperty(sb, "type", securityScheme.SecurityScheme.Type, indent + 4);

			SerializeSecurityDescriptor(sb, securityScheme.SecurityScheme, indent + 4);
			SerializeSecuritySettings(sb, securityScheme.Settings, indent + 4);
		}


	    private void SerializeSecuritySettings(StringBuilder sb, Settings settings, int indent)
		{
			if(settings == null)
				return;

			sb.AppendLine("settings:".Indent(indent));
            RamlSerializerHelper.SerializeProperty(sb, "accessTokenUri", settings.AccessTokenUri, indent + 2);
            RamlSerializerHelper.SerializeProperty(sb, "authorizationUri", settings.AuthorizationUri, indent + 2);
            RamlSerializerHelper.SerializeProperty(sb, "requestTokenUri", settings.RequestTokenUri, indent + 2);
            RamlSerializerHelper.SerializeProperty(sb, "tokenCredentialsUri", settings.TokenCredentialsUri, indent + 2);
			SerializeArrayProperty(sb, "authorizationGrants", settings.AuthorizationGrants, indent + 2);
            RamlSerializerHelper.SerializeListProperty(sb, "scopes", settings.Scopes.Select(s => s.Name).ToArray(), indent + 2);
		}



		private void SerializeSecurityDescriptor(StringBuilder sb, SecurityScheme describedBy, int indent)
		{
			if(describedBy == null)
				return;

			sb.AppendLine("securedBy:".Indent(indent));
			SerializeParameters(sb, "headers", describedBy.Headers, indent + 2);
			SerializeParameters(sb, "queryParameters", describedBy.QueryParameters, indent + 2);

			if (describedBy.Responses != null && describedBy.Responses.Any())
			{
				sb.AppendLine("responses:".Indent(indent + 2));
				foreach (var response in describedBy.Responses)
				{
					SerializeResponse(sb, response, indent + 4);
				}
			}
		}

		private static void SerializeProtocols(StringBuilder sb, IEnumerable<string> protocols, int indentation = 0)
		{
			if (protocols == null || !protocols.Any()) 
				return;

			sb.AppendFormat("protocols: {0}".Indent(indentation), "[" + string.Join(",", protocols) + "]");
			sb.AppendLine();
		}

		private void SerializeEndPoints(StringBuilder sb, IEnumerable<EndPoint> EndPoints, int indentation = 0)
		{
            if (EndPoints == null)
                return;

            var orderedEndpoints = EndPoints.OrderBy(e => e.Path).ToArray();
			foreach (var resource in orderedEndpoints)
			{
				SerializeResource(sb, resource, indentation);
			}
		}

		private void SerializeResource(StringBuilder sb, EndPoint resource, int indentation)
		{
			sb.AppendLine((resource.Path + ":").Indent(indentation));
			//SerializeParameters(sb, "baseUriParameters", resource.BaseUriParameters, indentation + 2);
            RamlSerializerHelper.SerializeDescriptionProperty(sb, resource.Description, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "displayName", resource.Name, indentation + 2);
			//SerializeProtocols(sb, resource.Protocols, indentation + 2);
			SerializeParameters(sb, "uriParameters", resource.Parameters?.Where(p => p.Binding == "URL"), indentation + 2);
            SerializeSecuritySchemes(sb, resource.Security);
			SerializeMethods(sb, resource.Operations, indentation + 2);
			//SerializeType(sb, resource.Type, indentation + 2);
			//SerializeEndPoints(sb, resource.EndPoints, indentation + 2);
            //raml1TypesSerializer.SerializeAnnotations(sb, resource.Annotations, indentation + 2);
		}



		private void SerializeMethods(StringBuilder sb, IEnumerable<Operation> methods, int indentation)
		{
            if (methods == null)
                return;

			foreach (var method in methods)
			{
				SerializeMethod(sb, method, indentation);
			}
		}

		private void SerializeMethod(StringBuilder sb, Operation method, int indentation)
		{
			sb.AppendLine((method.Method.ToLowerInvariant() + ":").Indent(indentation));
            RamlSerializerHelper.SerializeDescriptionProperty(sb, method.Description, indentation + 2);
			//SerializeType(sb, method.Type, indentation + 2);
			
			if (method.Request?.Headers != null)
			{
				sb.AppendLine("headers:".Indent(indentation + 2));
				foreach (var header in method.Request.Headers)
				{
                    sb.AppendLine(header.Name + ":".Indent(indentation + 4));
					RamlSerializerHelper.SerializeParameterProperties(sb, header, indentation + 6);
				}
			}

			//SerializeArrayProperty(sb, "is", method.Is, indentation + 2);
			SerializeProtocols(sb, method.Schemes, indentation + 2);
            if (method.Security != null)
            {
                foreach (var security in method.Security)
                {
                    SerializeSecurityDescriptor(sb, security, indentation + 2); // "securedBy"?
                }
            }
            //SerializeParameters(sb, "baseUriParameters", method.BaseUriParameters, indentation + 2);
			SerializeParameters(sb, "queryParameters", method.Request?.QueryParameters, indentation + 2);

            if (method.Request?.QueryString != null)
            {
                sb.AppendLine(("queryString:").Indent(indentation + 2));
                SerializeShape(sb, method.Request?.QueryString, indentation + 4);
            }

			SerializeBody(sb, method.Request?.Payloads, indentation + 2);
			SerializeResponses(sb, method.Responses, indentation + 2);
            //raml1TypesSerializer.SerializeAnnotations(sb, method.Annotations, indentation + 2);
		}

		private void SerializeBody(StringBuilder sb, IEnumerable<Payload> payloads, int indentation)
		{
			if(payloads == null || !payloads.Any())
				return;

			sb.AppendLine("body:".Indent(indentation));
			foreach (var payload in payloads)
			{
                SerializePayload(sb, payload, indentation + 2);
			}
		}

		private void SerializePayload(StringBuilder sb, Payload payload, int indentation)
        {
            sb.AppendLine((payload.MediaType + ":").Indent(indentation));
            var shape = payload.Schema;
            SerializeShape(sb, shape, indentation);
        }

        private static void SerializeShape(StringBuilder sb, Shape shape, int indentation)
        {
            if (shape == null)
                return;

            RamlSerializerHelper.SerializeDescriptionProperty(sb, shape.Description, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "type", Raml1TypesSerializer.GetShapeType(shape), indentation + 2);
            //SerializeParameters(sb, "formParameters", payload.Schema.FormParameters, indentation + 2);
            //RamlSerializerHelper.SerializeProperty(sb, "schema", payload.Schema, indentation + 2);
            //RamlSerializerHelper.SerializeProperty(sb, "examples", payload.Schema.Examples, indentation + 2);
            //raml1TypesSerializer.SerializeAnnotations(sb, payload.Schema.Annotations, indentation + 2);
        }

        private void SerializeResponses(StringBuilder sb, IEnumerable<Response> responses, int indentation)
		{
			if(responses == null || !responses.Any())
				return;

			sb.AppendLine("responses:".Indent(indentation));
			foreach (var response in responses)
			{
				SerializeResponse(sb, response, indentation + 2);
			}
		}

		private void SerializeResponse(StringBuilder sb, Response response, int indentation)
		{
			sb.AppendLine(response?.StatusCode.Indent(indentation) + ":");
            RamlSerializerHelper.SerializeDescriptionProperty(sb, response?.Description, indentation + 2);
            Raml1TypesSerializer.SerializeExamples(sb, response?.Examples, indentation + 2);
			SerializeBody(sb, response?.Payloads, indentation + 2);
		}

		private static void SerializeArrayProperty(StringBuilder sb, string enumerableTitle, IEnumerable<string> enumerable, int indentation = 0)
		{
			if(enumerable == null || !enumerable.Any())
				return;

			sb.AppendFormat((enumerableTitle + ": {0}").Indent(indentation), "[" + string.Join(",", enumerable) + "]");
			sb.AppendLine();
		}







		private void SerializeParameters(StringBuilder sb, string parametersTitle, IEnumerable<Parameter> parameters, int indentation = 0)
		{
			if (parameters == null || !parameters.Any())
				return;

			sb.AppendLine((parametersTitle + ":").Indent(indentation));
			foreach (var parameter in parameters)
			{
				SerializeParameter(sb, parameter, indentation + 2);
			}
		}

		private void SerializeParameter(StringBuilder sb, Parameter parameter, int indentation)
		{
			sb.AppendFormat("{0}:".Indent(indentation), parameter.Name);
			sb.AppendLine();

			RamlSerializerHelper.SerializeParameterProperties(sb, parameter, indentation);
		}



	}
}