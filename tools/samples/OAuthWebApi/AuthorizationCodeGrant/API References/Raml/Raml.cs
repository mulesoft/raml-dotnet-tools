
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
using Raml.Client.Common;
using Raml.Common;

namespace AuthorizationCodeGrant.Raml
{
    public partial class ApiMe
    {
        private readonly RamlApi proxy;

        internal ApiMe(RamlApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
		/// (Me.Get)
		/// </summary>
        public virtual async Task<ApiResponse> Get()
        {

            var url = "api/me";
	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");

            url += "?access_token=" + proxy.OAuthAccessToken;
            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            var headers = response.Headers;
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase
                                            };

        }

        /// <summary>
		/// (Me.Get)
		/// </summary>
		/// <param name="request">ApiRequest</param>
        public virtual async Task<ApiResponse> Get(ApiRequest request)
        {

            var url = "api/me";

	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");

            url += "?access_token=" + proxy.OAuthAccessToken;
            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase
                                            };
        }

    }

    /// <summary>
    /// Main class for grouping root resources. Nested resources are defined as properties. The constructor can optionally receive an URL and HttpClient instance to override the default ones.
    /// </summary>
    public partial class RamlApi
    {
        protected readonly HttpClient client;
        public const string BaseUri = "/";

        internal HttpClient Client { get { return client; } }


		private string oauthClientId;
		private string oauthClientSecret;
        private string tokenType = "bearer";


        public string OAuthAccessToken { get; set; }

		private string oauthAuthorizeUrl = "http://localhost:11625/OAuth/Authorize";
		public string OAuthAuthorizeUrl { get { return oauthAuthorizeUrl; } set { oauthAuthorizeUrl = value; } }

   		private string oauthAccessTokenUrl = "http://localhost:11625/OAuth/Token";
		public string OAuthAccessTokenUrl { get { return oauthAccessTokenUrl; } set { oauthAccessTokenUrl = value; } }

        public RamlApi(string endpointUrl)
        {
            if(string.IsNullOrWhiteSpace(endpointUrl))
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

            client = new HttpClient {BaseAddress = new Uri(endpointUrl)};
        }

        public RamlApi(HttpClient httpClient)
        {
            if(httpClient.BaseAddress == null)
                throw new InvalidOperationException("You must set the BaseAddress property of the HttpClient instance");

            client = httpClient;
        }

        
        public virtual ApiMe ApiMe
        {
            get { return new ApiMe(this); }
        }
                


		public void AddDefaultRequestHeader(string name, string value)
		{
			client.DefaultRequestHeaders.Add(name, value);
		}

		public void AddDefaultRequestHeader(string name, IEnumerable<string> values)
		{
			client.DefaultRequestHeaders.Add(name, values);
		}

		public void SetClientIdAndSecret(string key, string secret)
		{
			oauthClientId = key;
			oauthClientSecret = secret;
			tokenType = "bearer";
		}

        // grant_type=client_credentials
		public async virtual void GetApplicationOauthToken()
		{
			var data = WebUtility.UrlEncode(oauthClientId) + ":" + WebUtility.UrlEncode(oauthClientSecret);
			var request = new HttpRequestMessage(HttpMethod.Post, oauthAuthorizeUrl);
			request.Content = new FormUrlEncodedContent(new []{ new KeyValuePair<string, string>("grant_type", "client_credentials") });

			//request.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");
			request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(data)));
			var result = await client.SendAsync(request);
			var content = await result.Content.ReadAsStringAsync();
			dynamic obj = JsonConvert.DeserializeObject(content);
			OAuthAccessToken = obj.access_token;
			tokenType = obj.token_type;
		}

    }

} // end namespace




namespace AuthorizationCodeGrant.Raml.Models
{

} // end Models namespace