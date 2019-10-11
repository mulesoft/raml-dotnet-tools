using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Raml.Client.Common
{
	public class ApiResponse
	{
		public HttpStatusCode StatusCode { get; set; }
		public string ReasonPhrase { get; set; }
		public bool Ok { get { return StatusCode == HttpStatusCode.OK; } }
		public HttpResponseHeaders RawHeaders { get; set; }
		public HttpContent RawContent { get; set; }
		public IEnumerable<MediaTypeFormatter> Formatters { get; set; }
	}
}