using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser.Builders;
using Raml.Parser.Expressions;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class SecurityParserTests
    {
        [TestMethod]
        public void should_parse_whewn_oauth2()
        {
            var dynamicRaml = new Dictionary<string, object>();
            dynamicRaml.Add("title", "test");
            dynamicRaml.Add("baseUri", "http://baseuri.com");
            var oauth = new Dictionary<string, object>();
            
            var settings = new Dictionary<string, object>();
            var oauthSettings = new Dictionary<string, object>();
            oauthSettings.Add("authorizationUri","https://authorization.com");
            oauthSettings.Add("accessTokenUri", "https://accesstoken.com");
            oauthSettings.Add("authorizationGrants", new [] { "read", "write" });
            settings.Add("settings", oauthSettings);
            settings.Add("type", "OAuth 2.0");
            oauth.Add("oauth_2_0", settings);

            var securitySchemes = new object[] { oauth };
            dynamicRaml.Add("securitySchemes", securitySchemes);
            var ramlDocument = new RamlBuilder().Build(dynamicRaml).ConfigureAwait(false).GetAwaiter().GetResult();

            var security = SecurityParser.GetSecurity(ramlDocument);

            Assert.AreEqual("https://accesstoken.com", security.AccessTokenUri);
            Assert.AreEqual("read", security.AuthorizationGrants.First());
            Assert.AreEqual("https://authorization.com", security.AuthorizationUri);
        }

        [TestMethod]
        public void should_parse_whewn_oauth1()
        {
            var dynamicRaml = new Dictionary<string, object>();
            dynamicRaml.Add("title","test");
            dynamicRaml.Add("baseUri","http://baseuri.com");
            var oauth = new Dictionary<string, object>();
            var settings = new Dictionary<string, object>();
            var oauthSettings = new Dictionary<string, object>();
            oauthSettings.Add("authorizationUri", "https://authorization.com");
            oauthSettings.Add("tokenCredentialsUri", "https://tokencredentials.com");
            oauthSettings.Add("requestTokenUri", "https://requesttoken.com");
            settings.Add("settings", oauthSettings);
            settings.Add("type", "OAuth 1.0");
            oauth.Add("oauth_1_0", settings);
            var securitySchemes = new object[] { oauth };
            dynamicRaml.Add("securitySchemes", securitySchemes);
            var ramlDocument = new RamlBuilder().Build(dynamicRaml).ConfigureAwait(false).GetAwaiter().GetResult();

            var security = SecurityParser.GetSecurity(ramlDocument);

            Assert.AreEqual("https://tokencredentials.com", security.TokenCredentialsUri);
            Assert.AreEqual("https://requesttoken.com", security.RequestTokenUri);
            Assert.AreEqual("https://authorization.com", security.AuthorizationUri);
        }
    }
}