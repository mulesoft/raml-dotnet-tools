using System.Text.RegularExpressions;
using RAML.Parser.Model;
using AMF.Tools.Core.Pluralization;

namespace AMF.Tools.Core
{
    public class SchemaParameterParser
    {
        private readonly IPluralizationService pluralizationService;

        public SchemaParameterParser(IPluralizationService pluralizationService)
        {
            this.pluralizationService = pluralizationService;
        }

        public string Parse(string schema, EndPoint resource, Operation method, string fullUrl)
        {
            var url = GetResourcePath(resource, fullUrl);

            var res = ReplaceReservedParameters(schema, method, url);
            //res = ReplaceCustomParameters(resource, res);
            // res = ReplaceParametersWithFunctions(resource, res, url);

            return res;
        }

        private static string GetResourcePath(EndPoint resource, string fullUrl)
        {
            var url = resource.Path;
            url = ReplaceUriParameters(url);

            if (string.IsNullOrWhiteSpace(url) || url.Trim() == "/")
            {
                fullUrl = ReplaceUriParameters(fullUrl);
                url = fullUrl;
            }

            url = url.TrimEnd('/');
            if (!url.StartsWith("/"))
                url = "/" + url;
            return url;
        }

        private static string ReplaceReservedParameters(string schema, Operation operation, string url)
        {
            var res = schema.Replace("<<resourcePathName>>", url.Substring(1));
            res = res.Replace("<<resourcePath>>", url);
            if (operation != null && operation.Method != null)
                res = res.Replace("<<methodName>>", operation.Method.ToLower());
            return res;
        }

        //private static string ReplaceCustomParameters(EndPoint resource, string res)
        //{
        //    var regex = new Regex(@"\<\<([^>]+)\>\>", RegexOptions.IgnoreCase);
        //    var matchCollection = regex.Matches(res);
        //    foreach (Match match in matchCollection)
        //    {
        //        if(resource.Type == null)
        //            continue;

        //        var paramFound = match.Groups[1].Value;
        //        var type = resource.GetSingleType();
        //        if (string.IsNullOrWhiteSpace(type) || !resource.Type.ContainsKey(type) || resource.Type[type] == null || !resource.Type[type].ContainsKey(paramFound))
        //            continue;

        //        var value = resource.Type[type][paramFound];
        //        res = res.Replace("<<" + paramFound + ">>", value);
        //    }

        //    return res;
        //}

        //private string ReplaceParametersWithFunctions(EndPoint resource, string res, string url)
        //{
        //    var regex = new Regex(@"\<\<([a-z_-]+)\s?\|\s?\!(pluralize|singularize)\>\>", RegexOptions.IgnoreCase);
        //    var matchCollection = regex.Matches(res);
        //    foreach (Match match in matchCollection)
        //    {
        //        string replacementWord;
        //        switch (match.Groups[1].Value)
        //        {
        //            case "resourcePathName":
        //                replacementWord = url.Substring(1);
        //                break;
        //            case "resourcePath":
        //                replacementWord = url.Substring(1);
        //                break;
        //            case "methodName":
        //                replacementWord = url.Substring(1);
        //                break;
        //            default:
        //                var paramFound = match.Groups[1].Value;
        //                var type = resource.GetSingleType();
        //                if (string.IsNullOrWhiteSpace(type))
        //                    continue;

        //                if (!resource.Type[type].ContainsKey(paramFound))
        //                    continue;

        //                replacementWord = resource.Type[type][paramFound];
        //                break;
        //        }

        //        if (match.Groups[2].Value == "singularize")
        //        {
        //            res = res.Replace(match.Groups[0].Value, Singularize(replacementWord));
        //        }
        //        else if (match.Groups[2].Value == "pluralize")
        //        {
        //            res = res.Replace(match.Groups[0].Value, Pluralize(replacementWord));
        //        }
        //    }
        //    return res;
        //}

        private static string ReplaceUriParameters(string fullUrl)
        {
            var regexParams = new Regex(@"\{[^\}]+\}");
            fullUrl = regexParams.Replace(fullUrl, string.Empty);
            return fullUrl;
        }

        public string Singularize(string input)
        {
            return pluralizationService.Singularize(input);
        }

        public string Pluralize(string input)
        {
            return pluralizationService.Pluralize(input);
        }
    }
}