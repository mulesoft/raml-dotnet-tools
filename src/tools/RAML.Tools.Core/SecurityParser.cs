using System.Collections.Generic;
using System.Linq;
using RAML.Parser.Model;

namespace AMF.Tools.Core
{
    public class SecurityParser
    {
        private static ICollection<ParametrizedSecurityScheme> parametrizedSecuritySchemes = new List<ParametrizedSecurityScheme>();
        private static ICollection<SecurityScheme> securitySchemes = new List<SecurityScheme>();

        public static Security GetSecurity(WebApi ramlDocument)
        {
            if (ramlDocument.Security != null && ramlDocument.Security.Any())
            {
                foreach(var secReq in ramlDocument.Security)
                {
                    foreach (var sec in secReq.Schemes)
                    {
                        parametrizedSecuritySchemes.Add(sec);
                    }
                }
            }

            foreach(var secReq in ramlDocument.EndPoints.SelectMany(e => e.Operations).SelectMany(o => o.Security))
            {
                foreach (var sec in secReq.Schemes)
                {
                    securitySchemes.Add(sec.SecurityScheme);
                }
            }

            if (securitySchemes.Count == 0)
                return null;

            var securityScheme = securitySchemes.First(); //TODO: check

            var settings = securityScheme?.Settings;

            var flow = settings?.Flows?.FirstOrDefault();

			return new Security
			       {
				       AccessTokenUri = flow != null ? flow.AccessTokenUri : settings?.AccessTokenUri,
				       AuthorizationGrants = settings?.AuthorizationGrants.ToArray(),
				       AuthorizationUri = flow != null ? flow.AuthorizationUri : settings?.AuthorizationUri,
				       Scopes = (flow != null ? flow.Scopes : settings?.Scopes)?.Select(s => s.Name).ToArray(),
				       RequestTokenUri = settings?.RequestTokenUri,
				       TokenCredentialsUri = settings?.TokenCredentialsUri,
				       Headers = securityScheme?.Headers == null
					       ? new List<GeneratorParameter>()
					       : ParametersMapper.Map(securityScheme.Headers).ToList(),
				       QueryParameters = securityScheme?.QueryParameters == null
					       ? new List<GeneratorParameter>()
					       : ParametersMapper.Map(securityScheme.QueryParameters).ToList()
			       };
		}
    }
}