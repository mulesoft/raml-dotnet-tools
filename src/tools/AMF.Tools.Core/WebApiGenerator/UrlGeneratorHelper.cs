namespace AMF.Tools.Core.WebApiGenerator
{
    public static class UrlGeneratorHelper
    {
        private const string MediaTypeExtensionParameter = "{mediaTypeExtension}";

        public static string GetRelativeUri(string url, string controllerPrefix)
        {
            var controllerRoutePrefix = controllerPrefix.StartsWith("/") ? controllerPrefix.Substring(1) : controllerPrefix;

            if (controllerRoutePrefix.Trim() == string.Empty)
                return url;

            var relativeUri = url == controllerRoutePrefix ? string.Empty : url.Replace(AdaptedControllerRoutePrefix(controllerRoutePrefix), string.Empty);
            relativeUri = ReplaceMultipleMediaTypeExtensionParamaters(relativeUri);
            relativeUri = FixConsecutiveParameters(relativeUri);
            relativeUri = FixConsecutiveSlashes(relativeUri);
            relativeUri = relativeUri.StartsWith("/") ? relativeUri.Substring(1) : relativeUri;
            relativeUri = relativeUri.Trim(new []{'/'});
            return relativeUri;
        }

        private static string AdaptedControllerRoutePrefix(string controllerRoutePrefix)
        {
            if (controllerRoutePrefix.Contains("{"))
                return controllerRoutePrefix;

            return controllerRoutePrefix.EndsWith("/") ? controllerRoutePrefix : controllerRoutePrefix + "/";
        }

        public static string FixControllerRoutePrefix(string relativeUri)
        {
            relativeUri = relativeUri.Replace(MediaTypeExtensionParameter, string.Empty);
            relativeUri = FixConsecutiveParameters(relativeUri);
            relativeUri = FixConsecutiveSlashes(relativeUri);
            relativeUri = relativeUri.Trim(new[] { '/' });
            return relativeUri;
        }

        private static string FixConsecutiveSlashes(string relativeUri)
        {
            relativeUri = relativeUri.Replace("//", "/");
            return relativeUri;
        }

        private static string FixConsecutiveParameters(string relativeUri)
        {
            relativeUri = relativeUri.Replace("}{", "}/{");
            return relativeUri;
        }

        private static string ReplaceMultipleMediaTypeExtensionParamaters(string relativeUri)
        {
            if (relativeUri.IndexOf(MediaTypeExtensionParameter, System.StringComparison.Ordinal) !=
                relativeUri.LastIndexOf(MediaTypeExtensionParameter, System.StringComparison.Ordinal))
                relativeUri = ReplaceFirst(relativeUri, MediaTypeExtensionParameter, string.Empty);
            return relativeUri;
        }

        static string ReplaceFirst(string text, string search, string replace)
        {
            var pos = text.IndexOf(search);
            if (pos < 0)
                return text;

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string GetParentUri(string url, string relativeUri)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            if (!url.StartsWith("/"))
                url = "/" + url;

            return url.Replace(relativeUri, string.Empty);
        }
    }
}