using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace RAML.Api.Core
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public Lazy<SchemaValidationResults> SchemaValidation { get; set; }



        public HttpResponseHeaders RawHeaders { get; set; }
        public HttpContent RawContent { get; set; }
        public IEnumerable<MediaTypeFormatter> Formatters { get; set; }
    }
}