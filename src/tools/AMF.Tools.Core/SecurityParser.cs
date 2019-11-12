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
                foreach(var sec in ramlDocument.Security)
                {
                    parametrizedSecuritySchemes.Add(sec);
                }
            }

            foreach(var sec in ramlDocument.EndPoints.SelectMany(e => e.Operations).SelectMany(o => o.Security))
            {
                securitySchemes.Add(sec);
            }

            if (securitySchemes.Count == 0)
                return null;

            var securityScheme = securitySchemes.First(); //TODO: check

            var settings = securityScheme?.Settings;

			return new Security
			       {
				       AccessTokenUri = settings?.AccessTokenUri,
				       AuthorizationGrants = settings?.AuthorizationGrants.ToArray(),
				       AuthorizationUri = settings?.AuthorizationUri,
				       Scopes = settings?.Scopes.Select(s => s.Name).ToArray(),
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