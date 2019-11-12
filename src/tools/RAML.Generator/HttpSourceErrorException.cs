using System.Net.Http;

namespace AMF.CLI
{
    public class HttpSourceErrorException : HttpRequestException
    {
        public HttpSourceErrorException(string message) : base(message) { }
    }
}