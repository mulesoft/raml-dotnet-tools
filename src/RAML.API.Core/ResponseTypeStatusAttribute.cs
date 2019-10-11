using System;
using System.Net;

namespace RAML.Api.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ResponseTypeStatusAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTypeStatusAttribute"/> class.
        /// </summary>
        /// <param name="responseType">The response type.</param>
        /// <param name="status">The HTTP status code.</param>
        public ResponseTypeStatusAttribute(Type responseType, HttpStatusCode status)
        {
            ResponseType = responseType;
            StatusCode = status;
        }

        /// <summary>
        /// Gets the HTTP status code
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the response type.
        /// </summary>
        public Type ResponseType { get; private set; }
    }
}