using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using AMF.Parser.Model;

namespace RAML.WebApiExplorer
{
    public class ApiExplorerServiceVersion1 : ApiExplorerService
    {
        private readonly TypeToShapeConverter typeBuilder;

        public ApiExplorerServiceVersion1(IApiExplorer apiExplorer, string baseUri = null)
            : base(apiExplorer, baseUri)
        {
            typeBuilder = new TypeToShapeConverter();
        }

        protected override Shape AddType(Type type)
        {
            var typeName = typeBuilder.Convert(type);
            return typeName;
        }

        protected override Shape CreateMimeType(string type)
        {
            return new TypeToShapeConverter().Convert(type);
        }
    }
}