
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RAML.Api.Core;


namespace EPiServerServiceAPI
{
    /// <summary>
    /// Used to obtain access tokens for the EPiServer ServiceAPI
    /// </summary>
    public partial class Token
    {
        private readonly EpiApi proxy;

        internal Token(EpiApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// Used to obtain access tokens for the EPiServer ServiceAPI. request an access token for a username, password couplet - Authorize
        /// </summary>
        /// <param name="json"></param>
        public virtual async Task<Models.TokenPostResponse> Post(string json)
        {

            var url = "token";
            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new StringContent(json);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync(Models.TokenPostResponse.GetSchema(response.StatusCode), response.Content);
                }

            }

            return new Models.TokenPostResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid(Models.TokenPostResponse.GetSchema(response.StatusCode), response.Content), true)
            };

        }

        /// <summary>
        /// Used to obtain access tokens for the EPiServer ServiceAPI. request an access token for a username, password couplet - Authorize
        /// </summary>
        /// <param name="request">Models.TokenPostRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.TokenPostResponse> Post(Models.TokenPostRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "token";
            var req = new HttpRequestMessage(HttpMethod.Post, url);

            if (request.RawHeaders != null)
            {
                foreach (var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            req.Content = request.Content;
            var response = await proxy.Client.SendAsync(req);
            if (proxy.SchemaValidation.Enabled && proxy.SchemaValidation.RaiseExceptions)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync(Models.TokenPostResponse.GetSchema(response.StatusCode), response.Content);
                }

            }
            return new Models.TokenPostResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid(Models.TokenPostResponse.GetSchema(response.StatusCode), response.Content), true)
            };
        }

    }

    /// <summary>
    /// Versioning the EPiServer ServiceAPI
    /// </summary>
    public partial class Version
    {
        private readonly EpiApi proxy;

        internal Version(EpiApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// Versioning the EPiServer ServiceAPI. Obtain the version of the EPiServer ServiceAPI - Api Version
        /// </summary>
        public virtual async Task<Models.VersionGetResponse> Get()
        {

            var url = "version";
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
                throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{  \"$schema\": \"http://json-schema.org/draft-04/schema#\",  \"title\": \"API Version\",  \"description\": \"A Version description for the EPiServer ServiceAPI\",  \"type\": \"object\",  \"properties\": {    \"Component\": {      \"type\": \"string\"    },    \"Version\": {      \"type\": \"string\"    }  },  \"required\": [\"Component\", \"Version\"]}", response.Content);
                }

            }

            return new Models.VersionGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{  \"$schema\": \"http://json-schema.org/draft-04/schema#\",  \"title\": \"API Version\",  \"description\": \"A Version description for the EPiServer ServiceAPI\",  \"type\": \"object\",  \"properties\": {    \"Component\": {      \"type\": \"string\"    },    \"Version\": {      \"type\": \"string\"    }  },  \"required\": [\"Component\", \"Version\"]}", response.Content), true)
            };

        }

        /// <summary>
        /// Versioning the EPiServer ServiceAPI. Obtain the version of the EPiServer ServiceAPI - Api Version
        /// </summary>
        /// <param name="request">ApiRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.VersionGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "version";
            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
                throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            if (request.RawHeaders != null)
            {
                foreach (var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            var response = await proxy.Client.SendAsync(req);
            if (proxy.SchemaValidation.Enabled && proxy.SchemaValidation.RaiseExceptions)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{  \"$schema\": \"http://json-schema.org/draft-04/schema#\",  \"title\": \"API Version\",  \"description\": \"A Version description for the EPiServer ServiceAPI\",  \"type\": \"object\",  \"properties\": {    \"Component\": {      \"type\": \"string\"    },    \"Version\": {      \"type\": \"string\"    }  },  \"required\": [\"Component\", \"Version\"]}", response.Content);
                }

            }
            return new Models.VersionGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{  \"$schema\": \"http://json-schema.org/draft-04/schema#\",  \"title\": \"API Version\",  \"description\": \"A Version description for the EPiServer ServiceAPI\",  \"type\": \"object\",  \"properties\": {    \"Component\": {      \"type\": \"string\"    },    \"Version\": {      \"type\": \"string\"    }  },  \"required\": [\"Component\", \"Version\"]}", response.Content), true)
            };
        }

    }

    /// <summary>
    /// Main class for grouping root resources. Nested resources are defined as properties. The constructor can optionally receive an URL and HttpClient instance to override the default ones.
    /// </summary>
    public partial class EpiApi
    {

        protected readonly HttpClient client;
        public const string BaseUri = "https://localhost:443/episerverapi/";

        internal HttpClient Client { get { return client; } }

        public SchemaValidationSettings SchemaValidation { get; private set; }

        public string OAuthAccessToken { get; set; }

        private string oauthAuthorizeUrl = "https://localhost:8081/oauth/authorize";
        public string OAuthAuthorizeUrl { get { return oauthAuthorizeUrl; } set { oauthAuthorizeUrl = value; } }

        private string oauthAccessTokenUrl = "https://localhost/episerverapi/token";
        public string OAuthAccessTokenUrl { get { return oauthAccessTokenUrl; } set { oauthAccessTokenUrl = value; } }

        public EpiApi(string endpointUrl)
        {
            SchemaValidation = new SchemaValidationSettings
            {
                Enabled = true,
                RaiseExceptions = true
            };


            if (string.IsNullOrWhiteSpace(endpointUrl))
                throw new ArgumentException("You must specify the endpoint URL", "endpointUrl");

            if (endpointUrl.Contains("{"))
            {
                var regex = new Regex(@"\{([^\}]+)\}");
                var matches = regex.Matches(endpointUrl);
                var parameters = new List<string>();
                foreach (Match match in matches)
                {
                    parameters.Add(match.Groups[1].Value);
                }
                throw new InvalidOperationException("Please replace parameter/s " + string.Join(", ", parameters) + " in the URL before passing it to the constructor ");
            }

            client = new HttpClient { BaseAddress = new Uri(endpointUrl) };
        }

        public EpiApi(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
                throw new InvalidOperationException("You must set the BaseAddress property of the HttpClient instance");


            client = httpClient;

            SchemaValidation = new SchemaValidationSettings
            {
                Enabled = true,
                RaiseExceptions = true
            };
        }


        public virtual Token Token
        {
            get { return new Token(this); }
        }

        public virtual Version Version
        {
            get { return new Version(this); }
        }



        public void AddDefaultRequestHeader(string name, string value)
        {
            client.DefaultRequestHeaders.Add(name, value);
        }

        public void AddDefaultRequestHeader(string name, IEnumerable<string> values)
        {
            client.DefaultRequestHeaders.Add(name, values);
        }


    }

} // end namespace


namespace EPiServerServiceAPI.Models
{
    public partial class TokenPostOKResponseContent
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }

    } // end class

    public partial class TokenPostBadRequestResponseContent
    {
        public string Error { get; set; }

    } // end class

    public partial class VersionGetOKResponseContent
    {
        public string Component { get; set; }
        public string Version { get; set; }

    } // end class

    /// <summary>
    /// Multiple Response Types TokenPostOKResponseContent, TokenPostBadRequestResponseContent
    /// </summary>
    public partial class MultipleTokenPost : ApiMultipleResponse
    {

        static readonly Dictionary<HttpStatusCode, string> schemas = new Dictionary<HttpStatusCode, string>
        {
            { HttpStatusCode.OK, "{  \"$schema\": \"http://json-schema.org/draft-04/schema#\",  \"title\": \"Access Token\",  \"description\": \"An Access Token returned by a successful authorization request\",  \"type\": \"object\",  \"properties\": {    \"access_token\": {      \"type\": \"string\"    },    \"token_type\": {      \"type\": \"string\",      \"pattern\" : \"bearer\"    },    \"expires_in\": {      \"type\": \"integer\",      \"minimum\": 1    }  },  \"required\": [\"access_token\", \"token_type\", \"expires_in\"]}"},
            { HttpStatusCode.BadRequest, "{  \"$schema\": \"http://json-schema.org/draft-04/schema#\",  \"title\": \"Access Request Error\",  \"description\": \"When a token is not generated\",  \"type\": \"object\",  \"properties\": {    \"error\": {      \"type\": \"string\"    }  },  \"required\" : [\"error\"]}"},
        };

        public static string GetSchema(HttpStatusCode statusCode)
        {
            return schemas.ContainsKey(statusCode) ? schemas[statusCode] : string.Empty;
        }

        public MultipleTokenPost()
        {
            names.Add(HttpStatusCode.OK, "TokenPostOKResponseContent");
            types.Add(HttpStatusCode.OK, typeof(TokenPostOKResponseContent));
            names.Add(HttpStatusCode.BadRequest, "TokenPostBadRequestResponseContent");
            types.Add(HttpStatusCode.BadRequest, typeof(TokenPostBadRequestResponseContent));
        }
        /// <summary>
        /// access is granted and an access token returned 
        /// </summary>
        public TokenPostOKResponseContent TokenPostOKResponseContent { get; set; }
        /// <summary>
        /// access is not granted 
        /// </summary>
        public TokenPostBadRequestResponseContent TokenPostBadRequestResponseContent { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class Token
    /// </summary>
    public partial class TokenPostRequest : ApiRequest
    {
        public TokenPostRequest(HttpContent Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }

        /// <summary>
        /// Request content
        /// </summary>
        public HttpContent Content { get; set; }
        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Response object for method Post of class Token
    /// </summary>

    public partial class TokenPostResponse : ApiResponse
    {

        private MultipleTokenPost typedContent;
        /// <summary>
        /// Typed response content
        /// </summary>
        public MultipleTokenPost Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                typedContent = new MultipleTokenPost();
                var task = Formatters != null && Formatters.Any()
                            ? RawContent.ReadAsAsync(typedContent.GetTypeByStatusCode(StatusCode), Formatters)
                            : RawContent.ReadAsAsync(typedContent.GetTypeByStatusCode(StatusCode));
                task.Wait();
                var content = task.GetAwaiter().GetResult();
                typedContent.SetPropertyByStatusCode(StatusCode, content);
                return typedContent;
            }
        }

        public static string GetSchema(HttpStatusCode statusCode)
        {
            return MultipleTokenPost.GetSchema(statusCode);
        }


    } // end class

    /// <summary>
    /// Response object for method Get of class Version
    /// </summary>

    public partial class VersionGetResponse : ApiResponse
    {


        private VersionGetOKResponseContent typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public VersionGetOKResponseContent Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                var task = Formatters != null && Formatters.Any()
                            ? RawContent.ReadAsAsync<VersionGetOKResponseContent>(Formatters)
                            : RawContent.ReadAsAsync<VersionGetOKResponseContent>();
                task.Wait();
                typedContent = task.GetAwaiter().GetResult();
                return typedContent;
            }
        }

    } // end class


} // end Models namespace