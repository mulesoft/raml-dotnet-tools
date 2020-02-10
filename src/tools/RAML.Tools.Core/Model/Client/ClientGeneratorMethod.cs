using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RAML.Api.Core;

namespace AMF.Tools.Core.ClientGenerator
{
    [Serializable]
    public class ClientGeneratorMethod
    {
        public string ModelsNamespace { get; set; }

        public string Name { get; set; }
        public string ReturnType { get; set; }

        public string OkReturnType { get; set; }

        public ApiObject ReturnTypeObject { get; set; }

        public string RequestType { get; set; }
        public string ResponseType { get; set; }

        public IEnumerable<string> RequestContentTypes { get; set; }

        public bool RequestIsXml
        {
            get
            {
                return RequestContentTypes.Any(c => c.Contains("xml")) &&
                       !RequestContentTypes.Any(c => c.Contains("json"));
            }
        }

        public IEnumerable<string> ResponseContentTypes { get; set; }

        public string XmlComment
        {
            get
            {
                var xmlComment = string.Empty;
                if (!string.IsNullOrWhiteSpace(Comment))
                {

                    xmlComment += "/// <summary>\r\n" +
                                  "\t\t/// " + XmlCommentHelper.Escape(Comment) + "\r\n" +
                                  "\t\t/// </summary>\r\n";
                }

                xmlComment += "\t\t/// <param name=\"request\">" + XmlCommentHelper.Escape(RequestType) + "</param>\r\n";
                if (ReturnType != "string" && ReturnType != "HttpContent")
                    xmlComment += "\t\t/// <param name=\"responseFormatters\">response formatters</param>\r\n";

                if (!string.IsNullOrWhiteSpace(xmlComment))
                    xmlComment = xmlComment.Substring(0, xmlComment.Length - 2); // remove last new line

                return xmlComment;
            }
        }

        public string XmlSimpleComment
        {
            get
            {
                var xmlComment = string.Empty;
                if (!string.IsNullOrWhiteSpace(Comment))
                {

                    xmlComment += "/// <summary>\r\n" +
                                  "\t\t/// " + XmlCommentHelper.Escape(Comment) + "\r\n" +
                                  "\t\t/// </summary>\r\n";

                }

                if (HasInputParameter())
                    xmlComment += "\t\t/// <param name=\"" + Parameter.Name + "\">" + (Parameter.Description == null ? string.Empty : XmlCommentHelper.Escape(Parameter.Description)) + "</param>\r\n";

                if (UriParameters != null && UriParameters.Any())
                {
                    xmlComment = UriParameters.Aggregate(xmlComment,
                        (current, parameter) =>
                            current +
                            ("\t\t/// <param name=\"" + parameter.Name + "\">" + (parameter.Description == null ? string.Empty : XmlCommentHelper.Escape(parameter.Description)) + "</param>\r\n"));
                }

                if (Query != null)
                    xmlComment += "\t\t/// <param name=\"" + Query.Name.ToLowerInvariant() + "\">" + (Query.Description == null ? "query properties" : XmlCommentHelper.Escape(Query.Description)) + "</param>\r\n";

                if (!string.IsNullOrWhiteSpace(xmlComment))
                    xmlComment = xmlComment.Substring(0, xmlComment.Length - 2); // remove last new line

                return xmlComment;
            }
        }

        public GeneratorParameter Parameter { get; set; }
        public string Comment { get; set; }

        public string Url { get; set; }

        public string Verb { get; set; }

        public string NetHttpMethod
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Verb))
                    return "HttpMethod.Get";

                if (Verb == "Patch")
                    return "new HttpMethod(\"PATCH\")";

                return "HttpMethod." + Verb;
            }
        }

        public IEnumerable<GeneratorParameter> UriParameters
        {
            get
            {
                if (simpleParameterString == null)
                    SetSimpleParameterString();

                return _uriParameters;
            }
            set => _uriParameters = value;
        }

        public int UriParametersCount { get { return UriParameters == null ? 0 : UriParameters.Count(); } }

        public ClientGeneratorMethod Parent { get; set; }

        public ApiObject Query { get; set; }

        public ApiObject Header { get; set; }

        public IDictionary<string, ApiObject> ResponseHeaders { get; set; }

        public bool UseSecurity { get; set; }

        public string SimpleReturnTypeString
        {
            get
            {
                if (ReturnType == "string")
                    return "HttpContent";

                if (CollectionTypeHelper.IsCollection(ReturnType) || NewNetTypeMapper.IsPrimitiveType(Parameter.Type))
                    return ReturnType;

                return ModelsNamespace + "." + OkReturnType;
            }
        }

        public string SimpleParameterString
        {
            get
            {
                if (simpleParameterString == null)
                    SetSimpleParameterString();

                return simpleParameterString;
            }
        }

        public void SetSimpleParameterString()
        {
            var parameters = new Dictionary<string, string>();
            if (HasInputParameter())
            {
                var key = AddParameter(parameters, Parameter.Name,
                    ((CollectionTypeHelper.IsCollection(Parameter.Type) || NewNetTypeMapper.IsPrimitiveType(Parameter.Type))
                            ? Parameter.Type
                            : ModelsNamespace + "." + Parameter.Type));
            }

            if (_uriParameters != null)
            {
                foreach (var uriParam in _uriParameters)
                {
                    var key = AddParameter(parameters, uriParam.Name, uriParam.Type);
                    uriParam.ParamName = key;
                }
            }

            if (Query != null)
            {
                var key = AddParameter(parameters, Query.Name.ToLower(), ModelsNamespace + "." + Query.Name);
            }

            simpleParameterString = string.Join(", ", parameters.Select(p => p.Value).ToArray());
        }

        private int i = 1;
        private string simpleParameterString;
        private IEnumerable<GeneratorParameter> _uriParameters;

        private string AddParameter(Dictionary<string, string> paramKeys, string key, string value)
        {
            key = NetNamingMapper.RemoveInvalidChars(key);
            if (paramKeys.ContainsKey(key))
            {
                key += i;
                i++;
            }
            value += " " + key;
            paramKeys.Add(key, value);
            return key;
        }

        public string ParameterString
        {
            get
            {
                var paramsString = RequestType + " request";
                if (ReturnType != "string" && ReturnType != "HttpContent")
                    paramsString += ", IEnumerable<MediaTypeFormatter> responseFormatters = null";

                return paramsString;
            }
        }

        public string ParameterStringCore
        {
            get
            {
                var paramsString = RequestType + " request";
                if (ReturnType != "string" && ReturnType != "HttpContent")
                    paramsString += ", IEnumerable<IOutputFormatter> responseFormatters = null";

                return paramsString;
            }
        }

        public string QualifiedParameterType
        {
            get
            {
                if (!CollectionTypeHelper.IsCollection(Parameter.Type) && !NewNetTypeMapper.IsPrimitiveType(Parameter.Type))
                    return ModelsNamespace + "." + Parameter.Type;

                return Parameter.Type;
            }
        }

        public string ResponseHeaderType { get; set; }
        public string UriParametersType { get; set; }

        public bool HasInputParameter()
        {
            return (Verb == "Post" || Verb == "Put" || Verb == "Patch") && Parameter != null;
        }

    }
}