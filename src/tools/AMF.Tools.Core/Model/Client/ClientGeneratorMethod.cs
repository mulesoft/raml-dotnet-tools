using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AMF.Tools.Core.ClientGenerator
{
    [Serializable]
    public class ClientGeneratorMethod
    {
        public const string ModelsNamespacePrefix = "Models.";
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
                
                if(Verb == "Patch")
                    return "new HttpMethod(\"PATCH\")";

                return "HttpMethod." + Verb;
            }
        }

        public IEnumerable<GeneratorParameter> UriParameters { get; set; }

        public int UriParametersCount { get { return UriParameters == null ? 0 : UriParameters.Count(); } }

        public ClientGeneratorMethod Parent { get; set; }

        public string UriParametersString
        {
            get
            {
                if(UriParameters == null || !UriParameters.Any())
                    return string.Empty;

                return string.Join(", ", UriParameters.Select(p => p.Type + " " + p.Name).ToArray());
            }
        }

        public ApiObject Query { get; set; }

        public ApiObject Header { get; set; }

        public IDictionary<HttpStatusCode, ApiObject> ResponseHeaders { get; set; }

        public bool UseSecurity { get; set; }

        public string SimpleReturnTypeString
        {
            get
            {
                if (ReturnType == "string") 
                    return "HttpContent";

                if (CollectionTypeHelper.IsCollection(ReturnType))
                    return ReturnType;

                return ModelsNamespacePrefix + OkReturnType;
            }
        }

        public string SimpleParameterString
        {
            get
            {
                var paramsString = string.Empty;
                if (HasInputParameter())
                    paramsString += (Parameter.Type == "string" || CollectionTypeHelper.IsCollection(Parameter.Type)
                        ? Parameter.Type 
                        : ModelsNamespacePrefix + Parameter.Type) + " " + Parameter.Name;

                if (!string.IsNullOrWhiteSpace(UriParametersString))
                {
                    if (string.IsNullOrWhiteSpace(paramsString))
                        paramsString = UriParametersString;
                    else
                        paramsString += ", " + UriParametersString;
                }
                if (Query != null)
                {
                    if (string.IsNullOrWhiteSpace(paramsString))
                        paramsString = ModelsNamespacePrefix + Query.Name + " " + Query.Name.ToLower();
                    else
                        paramsString += ", " + ModelsNamespacePrefix + Query.Name + " " + Query.Name.ToLower();
                }

                return paramsString;
            }
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
                if (Parameter.Type != "string" && !CollectionTypeHelper.IsCollection(Parameter.Type))
                    return "Models." + Parameter.Type;

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