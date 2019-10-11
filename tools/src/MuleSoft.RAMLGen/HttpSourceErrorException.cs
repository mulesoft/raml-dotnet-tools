using System.Net.Http;

namespace MuleSoft.RAMLGen
{
    public class HttpSourceErrorException : HttpRequestException
    {
        public HttpSourceErrorException(string message) : base(message) { }
    }
}