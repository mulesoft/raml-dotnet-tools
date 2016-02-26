using System;
using System.Web.Http.Description;

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
            var typeName = type.Name.Replace("`", string.Empty);
            if (Types.Contains(type))
                return typeName;

            typeBuilder.Add(type);

            return typeName;
        }
    }
}