
namespace AMF.Common
{
    public class RamlProperties
    {
        public string Namespace { get; set; }

        public string Source { get; set; }

        public bool? UseAsyncMethods { get; set; }

        public string ClientName { get; set; }

        public bool? IncludeApiVersionInRoutePrefix { get; set; }

        public string ModelsFolder { get; set; }

        public string ImplementationControllersFolder { get; set; }

        public bool? AddGeneratedSuffix { get; set; }
    }
}