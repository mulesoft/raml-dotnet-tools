using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AMF.Parser.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RAML.WebApiExplorer.Tests
{
    internal class OasSerializer
    {
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        const int Tab = 2;
        internal string Serialize(AmfModel model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"swagger\": \"2.0\",".Indent(Tab));
            if (model.WebApi != null)
            {
                SerializeInfo(sb, model.WebApi, Tab);
            }
            SerializeProperty(sb, "host", model.WebApi.Host, Tab);
            SerializeProperty(sb, "basePath", model.WebApi.BasePath, Tab);
            SerializeArray(sb, "schemes", model.WebApi.Schemes, Tab);
            SerializeArray(sb, "consumes", model.WebApi.Accepts, Tab);
            SerializeArray(sb, "produces", model.WebApi.ContentType, Tab);
            SerializePaths(sb, model.WebApi.EndPoints);
            SerializeDefinitions(sb, model.Shapes);
            SerializeSecurity(sb, model.WebApi.Security);
            FixLastComma(sb);
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void SerializeArray(StringBuilder sb, string name, IEnumerable<string> array, int indentation)
        {
            if (array == null || array.Count() == 0)
                return;

            var jsonArray = JsonConvert.SerializeObject(array);
            sb.AppendLine($"\"{name}\": {jsonArray},".Indent(indentation));
        }

        private void SerializeSecurity(StringBuilder sb, IEnumerable<ParametrizedSecurityScheme> security)
        {
            
        }

        private void SerializeDefinitions(StringBuilder sb, IEnumerable<Shape> shapes)
        {
            
        }

        private void SerializePaths(StringBuilder sb, IEnumerable<EndPoint> endPoints)
        {
            
        }

        private void SerializeInfo(StringBuilder sb, WebApi webApi, int indentation)
        {
            if (string.IsNullOrWhiteSpace(webApi.Name) && string.IsNullOrWhiteSpace(webApi.Description)
                && string.IsNullOrWhiteSpace(webApi.TermsOfService) && webApi.Provider == null && webApi.License == null)
                return;

            sb.AppendLine("\"info\": {".Indent(indentation));
            SerializeProperty(sb, "title", webApi.Name, indentation + Tab);
            SerializeProperty(sb, "description", webApi.Description, indentation + Tab);
            SerializeProperty(sb, "termsOfService", webApi.TermsOfService, indentation + Tab);
            SerializeJsonObject(sb, "contact", webApi.Provider, indentation + Tab);
            SerializeJsonObject(sb, "license", webApi.License, indentation + Tab);
            FixLastComma(sb);
            sb.AppendLine("},".Indent(indentation));
        }

        private static void FixLastComma(StringBuilder sb)
        {
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            else if (sb[sb.Length - 3] == ',')
            {
                var length = ("," + Environment.NewLine).Length;
                sb.Remove(sb.Length - length, length);
            }

            sb.AppendLine();
        }

        private void SerializeJsonObject(StringBuilder sb, string name, Object obj, int indentation)
        {
            if (obj == null)
                return;

            sb.AppendLine($"\"{name}\":".Indent(indentation));
            var tw = new StringWriter(sb);

            var json = JsonConvert.SerializeObject(obj, serializerSettings) + ",";
            sb.Append(json.IndentLines(indentation));
            sb.AppendLine();
        }


        private void SerializeJsonObject<T>(StringBuilder sb, string name, Action<StringBuilder, T, int> serializeInner, T obj, int indentation)
        {
            sb.AppendLine($"\"{name}\": " + "{".Indent(indentation));
            serializeInner(sb, obj, indentation + 4);
            sb.AppendLine("},".Indent(indentation));
        }

        private void SerializeProperty(StringBuilder sb, string name, string value, int indentation)
        {
            if (value == null)
                return;

            sb.AppendLine($"\"{name}\": \"{value}\",".Indent(indentation));
        }
    }
}