using System.Collections.Generic;
using System.Linq;
using Raml.Parser.Expressions;

namespace Raml.Tools
{
    public class SecurityParser
    {
        private const string OAuth2Type = "OAuth 2.0";
        private const string OAuth1Type = "OAuth 1.0";

        public static Security GetSecurity(RamlDocument ramlDocument)
        {
            if (ramlDocument.SecuritySchemes == null || !ramlDocument.SecuritySchemes.Any())
                return null;

            SecurityScheme securityScheme = null;
            if (IsTypeDefined(ramlDocument, OAuth2Type))
            {
                securityScheme = GetSchemeWithType(ramlDocument, OAuth2Type);
            }
            else if (AreOAuth2EndpointsDefined(ramlDocument))
            {
                securityScheme = GetSchemeWithOAuth2EndpointsDefined(ramlDocument);
            }
            else if (IsTypeDefined(ramlDocument, OAuth1Type))
            {
                securityScheme = GetSchemeWithType(ramlDocument, OAuth1Type);
            }

            if (securityScheme == null || (securityScheme.Settings == null && securityScheme.DescribedBy == null))
                return new Security();

            var settings = securityScheme.Settings;
            var schemeDescriptor = securityScheme.DescribedBy;

			return new Security
			       {
				       AccessTokenUri = settings == null ? null : settings.AccessTokenUri,
				       AuthorizationGrants = settings == null ? null : settings.AuthorizationGrants.ToArray(),
				       AuthorizationUri = settings == null ? null : settings.AuthorizationUri,
				       Scopes = settings == null ? null : settings.Scopes.ToArray(),
				       RequestTokenUri = settings == null ? null : settings.RequestTokenUri,
				       TokenCredentialsUri = settings == null ? null : settings.TokenCredentialsUri,
				       Headers = schemeDescriptor == null || schemeDescriptor.Headers == null
					       ? new List<GeneratorParameter>()
					       : ParametersMapper.Map(schemeDescriptor.Headers).ToList(),
				       QueryParameters = schemeDescriptor == null || schemeDescriptor.QueryParameters == null
					       ? new List<GeneratorParameter>()
					       : ParametersMapper.Map(schemeDescriptor.QueryParameters).ToList()
			       };
		}

        private static SecurityScheme GetSchemeWithType(RamlDocument ramlDocument, string type)
        {

            return ramlDocument.SecuritySchemes.First(x => x.Values.Any(y => y.Type.First().Key == type))
                .Values
                .First(y => y.Type.First().Key == type);
        }

        private static bool IsTypeDefined(RamlDocument ramlDocument, string type)
        {
            return ramlDocument.SecuritySchemes.Any(x => x.Values.Any(y => y.Type.First().Key == type));
        }

        private static SecurityScheme GetSchemeWithOAuth2EndpointsDefined(RamlDocument ramlDocument)
        {
            return ramlDocument.SecuritySchemes.First(x => x.Values
                .Any(y => y.Settings != null
                          && !string.IsNullOrWhiteSpace(y.Settings.AuthorizationUri)
                          && !string.IsNullOrWhiteSpace(y.Settings.AccessTokenUri)))
                .Values
                .First(y => y.Settings != null
                            && !string.IsNullOrWhiteSpace(y.Settings.AuthorizationUri)
                            && !string.IsNullOrWhiteSpace(y.Settings.AccessTokenUri));
        }

        private static bool AreOAuth2EndpointsDefined(RamlDocument ramlDocument)
        {
            return ramlDocument.SecuritySchemes
                .Any(x => x.Values
                    .Any(y => y.Settings != null
                              && !string.IsNullOrWhiteSpace(y.Settings.AuthorizationUri)
                              && !string.IsNullOrWhiteSpace(y.Settings.AccessTokenUri)));
        }
    }
}