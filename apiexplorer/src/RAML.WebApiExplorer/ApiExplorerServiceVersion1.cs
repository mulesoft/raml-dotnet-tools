using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer
{
    public class ApiExplorerServiceVersion1 : ApiExplorerService
    {
        private readonly Raml1TypeBuilder typeBuilder;

        public ApiExplorerServiceVersion1(IApiExplorer apiExplorer, string baseUri = null)
            : base(apiExplorer, baseUri)
        {
            typeBuilder = new Raml1TypeBuilder(RamlTypes);
        }

        protected override string AddType(Type type)
        {
            var typeName = typeBuilder.Add(type);
            return typeName;
        }

        protected override MimeType CreateMimeType(string type)
        {
            return new MimeType
            {
                Type = type
            };
        }
    }
}