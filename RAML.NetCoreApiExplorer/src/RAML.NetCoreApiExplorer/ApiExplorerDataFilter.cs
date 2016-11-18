using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer
{
    /// <summary>
    /// A resource filter that looks up and serializes Api Explorer data for the action.
    ///
    /// This replaces the 'actual' output of the action.
    /// </summary>
    public class ApiExplorerDataFilter : IResourceFilter
    {
        private readonly IApiDescriptionGroupCollectionProvider _descriptionProvider;

        public ApiExplorerDataFilter(IApiDescriptionGroupCollectionProvider descriptionProvider)
        {
            _descriptionProvider = descriptionProvider;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!IsRamlController(context))
                return;

            if (!IsRawAction(context))
                return;

            var ramlVersion = GetRamlVersion(context);

            RamlDocument ramlDocument;
            if (ramlVersion == RamlVersion.Version1)
                ramlDocument = new ApiExplorerServiceVersion1(_descriptionProvider).GetRaml();
            else
                ramlDocument = new ApiExplorerServiceVersion08(_descriptionProvider).GetRaml(ramlVersion);
            var raml = new RamlSerializer().Serialize(ramlDocument);
            var result = new ContentResult
            {
                ContentType = "text/raml",
                Content = raml,
                StatusCode = 200
            };
            context.Result = result;
        }

        private static RamlVersion GetRamlVersion(ResourceExecutingContext context)
        {
            var ramlVersion = RamlVersion.Version1;

            if (context.HttpContext.Request.Query == null ||
                context.HttpContext.Request.Query.All(q => q.Key != "version"))
                return ramlVersion;

            var value = context.HttpContext.Request.Query["version"];
            var version = value.FirstOrDefault();
            if (version != "0.8")
                return ramlVersion;

            return RamlVersion.Version08;
        }

        private static bool IsRamlController(ActionContext context)
        {
            return (context.RouteData.Values.ContainsKey("controller") &&
                    context.RouteData.Values["controller"].ToString().ToLowerInvariant() == "raml");
        }

        private static bool IsRawAction(ActionContext context)
        {
            return context.RouteData.Values.ContainsKey("action") &&
                   context.RouteData.Values["action"].ToString().ToLowerInvariant() == "raw";
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

    }
}