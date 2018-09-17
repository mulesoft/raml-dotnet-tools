// Template: Client Proxy T4 Template (RAMLClient.t4) version 6.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AMF.Api.Core;
using MoviesClient.MoviesV1.Models;

namespace MoviesClient.MoviesV1
{
    public partial class Movies
    {
        private readonly MoviesV1Client proxy;

        internal Movies(MoviesV1Client proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// gets all movies in the catalogue - /movies
		/// </summary>
        public virtual async Task<Models.MoviesGetResponse> Get()
        {

            var url = "/movies";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.MoviesGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// gets all movies in the catalogue - /movies
		/// </summary>
		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.MoviesGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "/movies";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new Models.MoviesGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        /// <summary>
		/// adds a movie to the catalogue - /movies
		/// </summary>
		/// <param name="iListMovie"></param>
        public virtual async Task<ApiResponse> Post(IList<Movie> iListMovie)
        {

            var url = "/movies";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);
	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            req.Content = new ObjectContent(typeof(IList<Movie>), iListMovie, new JsonMediaTypeFormatter());                           
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// adds a movie to the catalogue - /movies
		/// </summary>
		/// <param name="request">Models.MoviesPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.MoviesPostRequest request)
        {

            var url = "/movies";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            if(request.Formatter == null)
                request.Formatter = new JsonMediaTypeFormatter();
	        var response = await proxy.Client.SendAsync(req);
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    public partial class MoviesId
    {
        private readonly MoviesV1Client proxy;

        internal MoviesId(MoviesV1Client proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// get the info of a movie - /movies/{id}
		/// </summary>
		/// <param name="id"></param>
        public virtual async Task<Models.MoviesIdGetResponse> Get(string id)
        {

            var url = "/movies/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.MoviesIdGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// get the info of a movie - /movies/{id}
		/// </summary>
		/// <param name="request">Models.MoviesIdGetRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.MoviesIdGetResponse> Get(Models.MoviesIdGetRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "/movies/{id}";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new Models.MoviesIdGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        /// <summary>
		/// update the info of a movie - /movies/{id}
		/// </summary>
		/// <param name="movie"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(Models.Movie movie, string id)
        {

            var url = "/movies/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);
            req.Content = new ObjectContent(typeof(Models.Movie), movie, new JsonMediaTypeFormatter());                           
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// update the info of a movie - /movies/{id}
		/// </summary>
		/// <param name="request">Models.MoviesIdPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.MoviesIdPutRequest request)
        {

            var url = "/movies/{id}";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            if(request.Formatter == null)
                request.Formatter = new JsonMediaTypeFormatter();
	        var response = await proxy.Client.SendAsync(req);
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        /// <summary>
		/// remove a movie from the catalogue - /movies/{id}
		/// </summary>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Delete(string id)
        {

            var url = "/movies/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Delete, url);
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// remove a movie from the catalogue - /movies/{id}
		/// </summary>
		/// <param name="request">Models.MoviesIdDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.MoviesIdDeleteRequest request)
        {

            var url = "/movies/{id}";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Delete, url);

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
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    /// <summary>
    /// rent a movie
    /// </summary>
    public partial class MoviesIdRent
    {
        private readonly MoviesV1Client proxy;

        internal MoviesIdRent(MoviesV1Client proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// rent a movie - /movies/{id}/rent
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(string content, string id)
        {

            var url = "/movies/{id}/rent";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);
	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            req.Content = new StringContent(content);
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// rent a movie - /movies/{id}/rent
		/// </summary>
		/// <param name="request">Models.MoviesIdRentPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.MoviesIdRentPutRequest request)
        {

            var url = "/movies/{id}/rent";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);

	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            req.Content = request.Content;
	        var response = await proxy.Client.SendAsync(req);
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    /// <summary>
    /// return a movie
    /// </summary>
    public partial class MoviesIdReturn
    {
        private readonly MoviesV1Client proxy;

        internal MoviesIdReturn(MoviesV1Client proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// return a movie - /movies/{id}/return
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(string content, string id)
        {

            var url = "/movies/{id}/return";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);
	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            req.Content = new StringContent(content);
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// return a movie - /movies/{id}/return
		/// </summary>
		/// <param name="request">Models.MoviesIdReturnPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.MoviesIdReturnPutRequest request)
        {

            var url = "/movies/{id}/return";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);

	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            req.Content = request.Content;
	        var response = await proxy.Client.SendAsync(req);
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    public partial class MoviesWishlist
    {
        private readonly MoviesV1Client proxy;

        internal MoviesWishlist(MoviesV1Client proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// gets the current user movies wishlist - /movies/wishlist
		/// </summary>
        public virtual async Task<Models.MoviesWishlistGetResponse> Get()
        {

            var url = "/movies/wishlist";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.MoviesWishlistGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// gets the current user movies wishlist - /movies/wishlist
		/// </summary>
		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.MoviesWishlistGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "/movies/wishlist";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);

	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new Models.MoviesWishlistGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    public partial class MoviesWishlistId
    {
        private readonly MoviesV1Client proxy;

        internal MoviesWishlistId(MoviesV1Client proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// add a movie to the current user movies wishlist - /movies/wishlist/{id}
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Post(string content, string id)
        {

            var url = "/movies/wishlist/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);
	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            req.Content = new StringContent(content);
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// add a movie to the current user movies wishlist - /movies/wishlist/{id}
		/// </summary>
		/// <param name="request">Models.MoviesWishlistIdPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.MoviesWishlistIdPostRequest request)
        {

            var url = "/movies/wishlist/{id}";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            req.Content = request.Content;
	        var response = await proxy.Client.SendAsync(req);
            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        /// <summary>
		/// removes a movie from the current user movies wishlist - /movies/wishlist/{id}
		/// </summary>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Delete(string id)
        {

            var url = "/movies/wishlist/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Delete, url);
	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
	        var response = await proxy.Client.SendAsync(req);

            return new ApiResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// removes a movie from the current user movies wishlist - /movies/wishlist/{id}
		/// </summary>
		/// <param name="request">Models.MoviesWishlistIdDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.MoviesWishlistIdDeleteRequest request)
        {

            var url = "/movies/wishlist/{id}";
			if(request.UriParameters == null)
				throw new InvalidOperationException("Uri Parameters cannot be null");               

			if(request.UriParameters.Id == null)
				throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Delete, url);

	        if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
				throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
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
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    public partial class MoviesRented
    {
        private readonly MoviesV1Client proxy;

        internal MoviesRented(MoviesV1Client proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// gets the user rented movies - /movies/rented
		/// </summary>
        public virtual async Task<Models.MoviesRentedGetResponse> Get()
        {

            var url = "/movies/rented";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.MoviesRentedGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// gets the user rented movies - /movies/rented
		/// </summary>
		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.MoviesRentedGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "/movies/rented";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new Models.MoviesRentedGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    public partial class MoviesAvailable
    {
        private readonly MoviesV1Client proxy;

        internal MoviesAvailable(MoviesV1Client proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// get all movies that are not currently rented - /movies/available
		/// </summary>
        public virtual async Task<Models.MoviesAvailableGetResponse> Get()
        {

            var url = "/movies/available";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.MoviesAvailableGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// get all movies that are not currently rented - /movies/available
		/// </summary>
		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.MoviesAvailableGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "/movies/available";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new Models.MoviesAvailableGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    public partial class Search
    {
        private readonly MoviesV1Client proxy;

        internal Search(MoviesV1Client proxy)
        {
            this.proxy = proxy;
        }


        /// <summary>
		/// search movies by name or director - /search
		/// </summary>
		/// <param name="getsearchquery">query properties</param>
        public virtual async Task<Models.SearchGetResponse> Get(Models.GetSearchQuery getsearchquery)
        {

            var url = "/search";
            if(getsearchquery != null)
            {
                url += "?";
                if(getsearchquery.Name != null)
					url += "&name=" + getsearchquery.Name;
                if(getsearchquery.Director != null)
					url += "&director=" + getsearchquery.Director;
            }

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.SearchGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        /// <summary>
		/// search movies by name or director - /search
		/// </summary>
		/// <param name="request">Models.SearchGetRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.SearchGetResponse> Get(Models.SearchGetRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "/search";
            if(request.Query != null)
            {
                url += "?";
                if(request.Query.Name != null)
                    url += "&name=" + request.Query.Name;
                if(request.Query.Director != null)
                    url += "&director=" + request.Query.Director;
            }

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if(request.RawHeaders != null)
            {
                foreach(var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
	        var response = await proxy.Client.SendAsync(req);
            return new Models.SearchGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }

    }

    /// <summary>
    /// Main class for grouping root resources. Nested resources are defined as properties. The constructor can optionally receive an URL and HttpClient instance to override the default ones.
    /// </summary>
    public partial class MoviesV1Client
    {

		public SchemaValidationSettings SchemaValidation { get; private set; } 

        protected readonly HttpClient client;
        public const string BaseUri = "http://movies.com/api/";

        internal HttpClient Client { get { return client; } }




        public string OAuthAccessToken { get; set; }

		private string oauthAuthorizeUrl = "https://localhost:8081/oauth/authorize";
		public string OAuthAuthorizeUrl { get { return oauthAuthorizeUrl; } set { oauthAuthorizeUrl = value; } }

   		private string oauthAccessTokenUrl = "https://localhost:8081/oauth/access_token";
		public string OAuthAccessTokenUrl { get { return oauthAccessTokenUrl; } set { oauthAccessTokenUrl = value; } }

        public MoviesV1Client(string endpointUrl)
        {
            SchemaValidation = new SchemaValidationSettings
			{
				Enabled = true,
				RaiseExceptions = true
			};

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

        public MoviesV1Client(HttpClient httpClient)
        {
            if(httpClient.BaseAddress == null)
                throw new InvalidOperationException("You must set the BaseAddress property of the HttpClient instance");

            client = httpClient;

			SchemaValidation = new SchemaValidationSettings
			{
				Enabled = true,
				RaiseExceptions = true
			};
        }

        

        public virtual Movies Movies
        {
            get { return new Movies(this); }
        }
                

        public virtual MoviesId MoviesId
        {
            get { return new MoviesId(this); }
        }
                

        public virtual MoviesIdRent MoviesIdRent
        {
            get { return new MoviesIdRent(this); }
        }
                

        public virtual MoviesIdReturn MoviesIdReturn
        {
            get { return new MoviesIdReturn(this); }
        }
                

        public virtual MoviesWishlist MoviesWishlist
        {
            get { return new MoviesWishlist(this); }
        }
                

        public virtual MoviesWishlistId MoviesWishlistId
        {
            get { return new MoviesWishlistId(this); }
        }
                

        public virtual MoviesRented MoviesRented
        {
            get { return new MoviesRented(this); }
        }
                

        public virtual MoviesAvailable MoviesAvailable
        {
            get { return new MoviesAvailable(this); }
        }
                

        public virtual Search Search
        {
            get { return new Search(this); }
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









namespace MoviesClient.MoviesV1.Models
{
    public partial class  Movie 
    {

        public int Id { get; set; }


        public string Name { get; set; }


        public string Director { get; set; }


        public string Genre { get; set; }


        public string Cast { get; set; }


        public decimal Duration { get; set; }


        public string Storyline { get; set; }


        public string Language { get; set; }


        public bool Rented { get; set; }


    } // end class

    public partial class  GetSearchQuery 
    {

        /// <summary>
        /// Name of the movie
        /// </summary>
		[JsonProperty("name")]
        public string Name { get; set; }


        /// <summary>
        /// Director of the movie
        /// </summary>
		[JsonProperty("director")]
        public string Director { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /movies/{id}
    /// </summary>
    public partial class  MoviesIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /movies/{id}/rent
    /// </summary>
    public partial class  MoviesIdRentUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /movies/{id}/return
    /// </summary>
    public partial class  MoviesIdReturnUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /movies/wishlist/{id}
    /// </summary>
    public partial class  MoviesWishlistIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Request object for method Post of class Movies
    /// </summary>
    public partial class MoviesPostRequest : ApiRequest
    {
        public MoviesPostRequest(IList<Movie> Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public IList<Movie> Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Get of class MoviesId
    /// </summary>
    public partial class MoviesIdGetRequest : ApiRequest
    {
        public MoviesIdGetRequest(MoviesIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class MoviesId
    /// </summary>
    public partial class MoviesIdPutRequest : ApiRequest
    {
        public MoviesIdPutRequest(MoviesIdUriParameters UriParameters, Movie Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Movie Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Delete of class MoviesId
    /// </summary>
    public partial class MoviesIdDeleteRequest : ApiRequest
    {
        public MoviesIdDeleteRequest(MoviesIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class MoviesIdRent
    /// </summary>
    public partial class MoviesIdRentPutRequest : ApiRequest
    {
        public MoviesIdRentPutRequest(MoviesIdRentUriParameters UriParameters, HttpContent Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public HttpContent Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesIdRentUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class MoviesIdReturn
    /// </summary>
    public partial class MoviesIdReturnPutRequest : ApiRequest
    {
        public MoviesIdReturnPutRequest(MoviesIdReturnUriParameters UriParameters, HttpContent Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public HttpContent Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesIdReturnUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class MoviesWishlistId
    /// </summary>
    public partial class MoviesWishlistIdPostRequest : ApiRequest
    {
        public MoviesWishlistIdPostRequest(MoviesWishlistIdUriParameters UriParameters, HttpContent Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public HttpContent Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesWishlistIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Delete of class MoviesWishlistId
    /// </summary>
    public partial class MoviesWishlistIdDeleteRequest : ApiRequest
    {
        public MoviesWishlistIdDeleteRequest(MoviesWishlistIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesWishlistIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Get of class Search
    /// </summary>
    public partial class SearchGetRequest : ApiRequest
    {
        public SearchGetRequest(GetSearchQuery Query = null)
        {
            this.Query = Query;
        }


        /// <summary>
        /// Request query string properties
        /// </summary>
        public GetSearchQuery Query { get; set; }

    } // end class

    /// <summary>
    /// Response object for method Get of class Movies
    /// </summary>

    public partial class MoviesGetResponse : ApiResponse
    {


	    private IList<Movie> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Movie> Content 
    	{
	        get
	        {
		        if (typedContent != null)
			        return typedContent;

                IEnumerable<string> values = new List<string>();
                if (RawContent != null && RawContent.Headers != null)
                    RawContent.Headers.TryGetValues("Content-Type", out values);

                if (values.Any(hv => hv.ToLowerInvariant().Contains("xml")) &&
                    !values.Any(hv => hv.ToLowerInvariant().Contains("json")))
                {
                    var task = RawContent.ReadAsStreamAsync();

                    var xmlStream = task.GetAwaiter().GetResult();
                    typedContent = (IList<Movie>)new XmlSerializer(typeof(IList<Movie>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Movie>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Movie>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class MoviesId
    /// </summary>

    public partial class MoviesIdGetResponse : ApiResponse
    {


	    private Movie typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public Movie Content 
    	{
	        get
	        {
		        if (typedContent != null)
			        return typedContent;

                IEnumerable<string> values = new List<string>();
                if (RawContent != null && RawContent.Headers != null)
                    RawContent.Headers.TryGetValues("Content-Type", out values);

                if (values.Any(hv => hv.ToLowerInvariant().Contains("xml")) &&
                    !values.Any(hv => hv.ToLowerInvariant().Contains("json")))
                {
                    var task = RawContent.ReadAsStreamAsync();

                    var xmlStream = task.GetAwaiter().GetResult();
                    typedContent = (Movie)new XmlSerializer(typeof(Movie)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<Movie>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<Movie>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class MoviesWishlist
    /// </summary>

    public partial class MoviesWishlistGetResponse : ApiResponse
    {


	    private IList<Movie> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Movie> Content 
    	{
	        get
	        {
		        if (typedContent != null)
			        return typedContent;

                IEnumerable<string> values = new List<string>();
                if (RawContent != null && RawContent.Headers != null)
                    RawContent.Headers.TryGetValues("Content-Type", out values);

                if (values.Any(hv => hv.ToLowerInvariant().Contains("xml")) &&
                    !values.Any(hv => hv.ToLowerInvariant().Contains("json")))
                {
                    var task = RawContent.ReadAsStreamAsync();

                    var xmlStream = task.GetAwaiter().GetResult();
                    typedContent = (IList<Movie>)new XmlSerializer(typeof(IList<Movie>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Movie>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Movie>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class MoviesRented
    /// </summary>

    public partial class MoviesRentedGetResponse : ApiResponse
    {


	    private IList<Movie> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Movie> Content 
    	{
	        get
	        {
		        if (typedContent != null)
			        return typedContent;

                IEnumerable<string> values = new List<string>();
                if (RawContent != null && RawContent.Headers != null)
                    RawContent.Headers.TryGetValues("Content-Type", out values);

                if (values.Any(hv => hv.ToLowerInvariant().Contains("xml")) &&
                    !values.Any(hv => hv.ToLowerInvariant().Contains("json")))
                {
                    var task = RawContent.ReadAsStreamAsync();

                    var xmlStream = task.GetAwaiter().GetResult();
                    typedContent = (IList<Movie>)new XmlSerializer(typeof(IList<Movie>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Movie>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Movie>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class MoviesAvailable
    /// </summary>

    public partial class MoviesAvailableGetResponse : ApiResponse
    {


	    private IList<Movie> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Movie> Content 
    	{
	        get
	        {
		        if (typedContent != null)
			        return typedContent;

                IEnumerable<string> values = new List<string>();
                if (RawContent != null && RawContent.Headers != null)
                    RawContent.Headers.TryGetValues("Content-Type", out values);

                if (values.Any(hv => hv.ToLowerInvariant().Contains("xml")) &&
                    !values.Any(hv => hv.ToLowerInvariant().Contains("json")))
                {
                    var task = RawContent.ReadAsStreamAsync();

                    var xmlStream = task.GetAwaiter().GetResult();
                    typedContent = (IList<Movie>)new XmlSerializer(typeof(IList<Movie>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Movie>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Movie>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class Search
    /// </summary>

    public partial class SearchGetResponse : ApiResponse
    {


	    private IList<Movie> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Movie> Content 
    	{
	        get
	        {
		        if (typedContent != null)
			        return typedContent;

                IEnumerable<string> values = new List<string>();
                if (RawContent != null && RawContent.Headers != null)
                    RawContent.Headers.TryGetValues("Content-Type", out values);

                if (values.Any(hv => hv.ToLowerInvariant().Contains("xml")) &&
                    !values.Any(hv => hv.ToLowerInvariant().Contains("json")))
                {
                    var task = RawContent.ReadAsStreamAsync();

                    var xmlStream = task.GetAwaiter().GetResult();
                    typedContent = (IList<Movie>)new XmlSerializer(typeof(IList<Movie>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Movie>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Movie>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class


} // end Models namespace
