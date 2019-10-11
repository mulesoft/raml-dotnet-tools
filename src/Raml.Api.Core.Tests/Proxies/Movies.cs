
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
using Raml.Common;


namespace Movies
{
    /// <summary>
    /// rent a movie
    /// </summary>
    public partial class Rent
    {
        private readonly MoviesApi proxy;

        internal Rent(MoviesApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// rent a movie
        /// </summary>
        /// <param name="json"></param>
        /// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(string json, string id)
        {

            var url = "movies/{id}/rent";
            url = url.Replace("{id}", id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Put, url);
            if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
                throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            req.Content = new StringContent(json);
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
        /// rent a movie
        /// </summary>
        /// <param name="request">Models.RentPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.RentPutRequest request)
        {

            var url = "movies/{id}/rent";
            if (request.UriParameters == null)
                throw new InvalidOperationException("Uri Parameters cannot be null");

            if (request.UriParameters.Id == null)
                throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Put, url);

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
    public partial class Return
    {
        private readonly MoviesApi proxy;

        internal Return(MoviesApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// return a movie
        /// </summary>
        /// <param name="json"></param>
        /// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(string json, string id)
        {

            var url = "movies/{id}/return";
            url = url.Replace("{id}", id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Put, url);
            if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
                throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            req.Content = new StringContent(json);
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
        /// return a movie
        /// </summary>
        /// <param name="request">Models.ReturnPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.ReturnPutRequest request)
        {

            var url = "movies/{id}/return";
            if (request.UriParameters == null)
                throw new InvalidOperationException("Uri Parameters cannot be null");

            if (request.UriParameters.Id == null)
                throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Put, url);

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

    public partial class Wishlist
    {
        private readonly MoviesApi proxy;

        internal Wishlist(MoviesApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// gets the current user movies wishlist
        /// </summary>
        public virtual async Task<Models.WishlistGetResponse> Get()
        {

            var url = "movies/wishlist";
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
                throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content);
                }

            }

            return new Models.WishlistGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content), true)
            };

        }

        /// <summary>
        /// gets the current user movies wishlist
        /// </summary>
        /// <param name="request">ApiRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.WishlistGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "movies/wishlist";
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
                    await SchemaValidator.ValidateWithExceptionAsync("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content);
                }

            }
            return new Models.WishlistGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content), true)
            };
        }

        /// <summary>
        /// add a movie to the current user movies wishlist
        /// </summary>
        /// <param name="json"></param>
        /// <param name="id"></param>
        public virtual async Task<ApiResponse> Post(string json, string id)
        {

            var url = "movies/wishlist/{id}";
            url = url.Replace("{id}", id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Post, url);
            if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
                throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            req.Content = new StringContent(json);
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
        /// add a movie to the current user movies wishlist
        /// </summary>
        /// <param name="request">Models.WishlistPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.WishlistPostRequest request)
        {

            var url = "movies/wishlist/{id}";
            if (request.UriParameters == null)
                throw new InvalidOperationException("Uri Parameters cannot be null");

            if (request.UriParameters.Id == null)
                throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Post, url);

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
        /// removes a movie from the current user movies wishlist
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task<ApiResponse> Delete(string id)
        {

            var url = "movies/wishlist/{id}";
            url = url.Replace("{id}", id.ToString());
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
        /// removes a movie from the current user movies wishlist
        /// </summary>
        /// <param name="request">Models.WishlistDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.WishlistDeleteRequest request)
        {

            var url = "movies/wishlist/{id}";
            if (request.UriParameters == null)
                throw new InvalidOperationException("Uri Parameters cannot be null");

            if (request.UriParameters.Id == null)
                throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Delete, url);

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

    public partial class Rented
    {
        private readonly MoviesApi proxy;

        internal Rented(MoviesApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// gets the user rented movies
        /// </summary>
        public virtual async Task<Models.RentedGetResponse> Get()
        {

            var url = "movies/rented";
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content);
                }

            }

            return new Models.RentedGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content), true)
            };

        }

        /// <summary>
        /// gets the user rented movies
        /// </summary>
        /// <param name="request">ApiRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.RentedGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "movies/rented";
            var req = new HttpRequestMessage(HttpMethod.Get, url);

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
                    await SchemaValidator.ValidateWithExceptionAsync("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content);
                }

            }
            return new Models.RentedGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content), true)
            };
        }

    }

    public partial class Available
    {
        private readonly MoviesApi proxy;

        internal Available(MoviesApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// get all movies that are not currently rented
        /// </summary>
        public virtual async Task<Models.AvailableGetResponse> Get()
        {

            var url = "movies/available";
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content);
                }

            }

            return new Models.AvailableGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content), true)
            };

        }

        /// <summary>
        /// get all movies that are not currently rented
        /// </summary>
        /// <param name="request">ApiRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.AvailableGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "movies/available";
            var req = new HttpRequestMessage(HttpMethod.Get, url);

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
                    await SchemaValidator.ValidateWithExceptionAsync("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content);
                }

            }
            return new Models.AvailableGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content), true)
            };
        }

    }

    public partial class Movies
    {
        private readonly MoviesApi proxy;

        internal Movies(MoviesApi proxy)
        {
            this.proxy = proxy;
        }
        public virtual Wishlist Wishlist
        {
            get { return new Wishlist(proxy); }
        }
        public virtual Rented Rented
        {
            get { return new Rented(proxy); }
        }
        public virtual Available Available
        {
            get { return new Available(proxy); }
        }

        /// <summary>
        /// gets all movies in the catalogue
        /// </summary>
        public virtual async Task<Models.MoviesGetResponse> Get()
        {

            var url = "movies";
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync(Models.MoviesGetResponse.GetSchema(response.StatusCode), response.Content);
                }

            }

            return new Models.MoviesGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid(Models.MoviesGetResponse.GetSchema(response.StatusCode), response.Content), true)
            };

        }

        /// <summary>
        /// gets all movies in the catalogue
        /// </summary>
        /// <param name="request">Models.MoviesGetRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.MoviesGetResponse> Get(Models.MoviesGetRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "movies";
            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if (request.Headers != null)
            {
                foreach (var header in request.Headers.Headers)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
            var response = await proxy.Client.SendAsync(req);
            if (proxy.SchemaValidation.Enabled && proxy.SchemaValidation.RaiseExceptions)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync(Models.MoviesGetResponse.GetSchema(response.StatusCode), response.Content);
                }

            }
            return new Models.MoviesGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid(Models.MoviesGetResponse.GetSchema(response.StatusCode), response.Content), true)
            };
        }

        /// <summary>
        /// adds a movie to the catalogue
        /// </summary>
        /// <param name="moviespostrequestcontent"></param>
        public virtual async Task<ApiResponse> Post(Models.MoviesPostRequestContent moviespostrequestcontent)
        {

            var url = "movies";
            var req = new HttpRequestMessage(HttpMethod.Post, url);
            if (string.IsNullOrEmpty(proxy.OAuthAccessToken))
                throw new InvalidOperationException("This API call is secured with OAuth, you must provide an access token (set OAuthAccessToken before calling this method)");
            req.Headers.Add("Authorization", "Bearer " + proxy.OAuthAccessToken);
            req.Content = new ObjectContent(typeof(Models.MoviesPostRequestContent), moviespostrequestcontent, new JsonMediaTypeFormatter());
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
        /// adds a movie to the catalogue
        /// </summary>
        /// <param name="request">Models.MoviesPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.MoviesPostRequest request)
        {

            var url = "movies";
            var req = new HttpRequestMessage(HttpMethod.Post, url);

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
            if (request.Formatter == null)
                request.Formatter = new JsonMediaTypeFormatter();

            req.Content = new ObjectContent(typeof(Models.MoviesPostRequestContent), request.Content, request.Formatter);
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
        /// get the info of a movie
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task<Models.MoviesGetByIdResponse> GetById(string id)
        {

            var url = "movies/{id}";
            url = url.Replace("{id}", id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync(Models.MoviesGetByIdResponse.GetSchema(response.StatusCode), response.Content);
                }

            }

            var headers = new Models.MultipleGetByIdHeader();
            headers.SetProperties(response.Headers, response.StatusCode);
            return new Models.MoviesGetByIdResponse
            {
                RawContent = response.Content,
                Headers = headers,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid(Models.MoviesGetByIdResponse.GetSchema(response.StatusCode), response.Content), true)
            };

        }

        /// <summary>
        /// get the info of a movie
        /// </summary>
        /// <param name="request">Models.MoviesGetByIdRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.MoviesGetByIdResponse> GetById(Models.MoviesGetByIdRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "movies/{id}";
            if (request.UriParameters == null)
                throw new InvalidOperationException("Uri Parameters cannot be null");

            if (request.UriParameters.Id == null)
                throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Get, url);

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
                    await SchemaValidator.ValidateWithExceptionAsync(Models.MoviesGetByIdResponse.GetSchema(response.StatusCode), response.Content);
                }

            }
            var headers = new Models.MultipleGetByIdHeader();
            headers.SetProperties(response.Headers, response.StatusCode);
            return new Models.MoviesGetByIdResponse
            {
                RawContent = response.Content,
                Headers = headers,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid(Models.MoviesGetByIdResponse.GetSchema(response.StatusCode), response.Content), true)
            };
        }

        /// <summary>
        /// update the info of a movie
        /// </summary>
        /// <param name="idputrequestcontent"></param>
        /// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(Models.IdPutRequestContent idputrequestcontent, string id)
        {

            var url = "movies/{id}";
            url = url.Replace("{id}", id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Put, url);
            req.Content = new ObjectContent(typeof(Models.IdPutRequestContent), idputrequestcontent, new JsonMediaTypeFormatter());
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
        /// update the info of a movie
        /// </summary>
        /// <param name="request">Models.MoviesPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.MoviesPutRequest request)
        {

            var url = "movies/{id}";
            if (request.UriParameters == null)
                throw new InvalidOperationException("Uri Parameters cannot be null");

            if (request.UriParameters.Id == null)
                throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Put, url);

            if (request.RawHeaders != null)
            {
                foreach (var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            if (request.Formatter == null)
                request.Formatter = new JsonMediaTypeFormatter();

            req.Content = new ObjectContent(typeof(Models.IdPutRequestContent), request.Content, request.Formatter);
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
        /// remove a movie from the catalogue
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task<ApiResponse> Delete(string id)
        {

            var url = "movies/{id}";
            url = url.Replace("{id}", id.ToString());
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
        /// remove a movie from the catalogue
        /// </summary>
        /// <param name="request">Models.MoviesDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.MoviesDeleteRequest request)
        {

            var url = "movies/{id}";
            if (request.UriParameters == null)
                throw new InvalidOperationException("Uri Parameters cannot be null");

            if (request.UriParameters.Id == null)
                throw new InvalidOperationException("Uri Parameter Id cannot be null");

            url = url.Replace("{id}", request.UriParameters.Id.ToString());
            var req = new HttpRequestMessage(HttpMethod.Delete, url);

            if (request.RawHeaders != null)
            {
                foreach (var header in request.RawHeaders)
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

    public partial class Search
    {
        private readonly MoviesApi proxy;

        internal Search(MoviesApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// search movies by name or director
        /// </summary>
        /// <param name="json"></param>
        /// <param name="postsearchquery">query properties</param>
        public virtual async Task<Models.SearchPostResponse> Post(string json, Models.PostSearchQuery postsearchquery)
        {

            var url = "search";
            if (postsearchquery != null)
            {
                url += "?";
                if (postsearchquery.Name != null)
                    url += "&postsearchquery.Name=" + postsearchquery.Name;
                if (postsearchquery.Director != null)
                    url += "&postsearchquery.Director=" + postsearchquery.Director;
            }
            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new StringContent(json);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content);
                }

            }

            return new Models.SearchPostResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content), true)
            };

        }

        /// <summary>
        /// search movies by name or director
        /// </summary>
        /// <param name="request">Models.SearchPostRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.SearchPostResponse> Post(Models.SearchPostRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "search";
            if (request.Query != null)
            {
                url += "?";
                if (request.Query.Name != null)
                    url += "&Name=" + request.Query.Name;
                if (request.Query.Director != null)
                    url += "&Director=" + request.Query.Director;
            }
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
                    await SchemaValidator.ValidateWithExceptionAsync("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content);
                }

            }
            return new Models.SearchPostResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": \"string\" },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": \"number\" },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}", response.Content), true)
            };
        }

    }

    /// <summary>
    /// Main class for grouping root resources. Nested resources are defined as properties. The constructor can optionally receive an URL and HttpClient instance to override the default ones.
    /// </summary>
    public partial class MoviesApi
    {

        protected readonly HttpClient client;
        public const string BaseUri = "http://movies.com/api/";

        internal HttpClient Client { get { return client; } }

        public SchemaValidationSettings SchemaValidation { get; private set; }

        public string OAuthAccessToken { get; set; }

        private string oauthAuthorizeUrl = "https://localhost:8081/oauth/authorize";
        public string OAuthAuthorizeUrl { get { return oauthAuthorizeUrl; } set { oauthAuthorizeUrl = value; } }

        private string oauthAccessTokenUrl = "https://localhost:8081/oauth/access_token";
        public string OAuthAccessTokenUrl { get { return oauthAccessTokenUrl; } set { oauthAccessTokenUrl = value; } }

        public MoviesApi(string endpointUrl)
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

        public MoviesApi(HttpClient httpClient)
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


        public virtual Movies Movies
        {
            get { return new Movies(this); }
        }

        public virtual Rent Rent
        {
            get { return new Rent(this); }
        }

        public virtual Return Return
        {
            get { return new Return(this); }
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


namespace Movies.Models
{
    public partial class MoviesPostRequestContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public string Cast { get; set; }
        public decimal? Duration { get; set; }
        public string Storyline { get; set; }
        public string Language { get; set; }

    } // end class

    public partial class IdPutRequestContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public string Cast { get; set; }
        public decimal Duration { get; set; }
        public string Storyline { get; set; }
        public string Language { get; set; }

    } // end class

    public partial class MoviesGetOKResponseContent
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

    public partial class MoviesGetBadRequestResponseContent
    {
        public string Error { get; set; }
        public int Code { get; set; }

    } // end class

    public partial class IdGetOKResponseContent
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

    public partial class IdGetBadRequestResponseContent
    {
        public string Error { get; set; }
        public int Code { get; set; }

    } // end class

    public partial class IdGetNotFoundResponseContent
    {
        public int Id { get; set; }
        public string Error { get; set; }
        public int Code { get; set; }

    } // end class

    public partial class WishlistGetOKResponseContent
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

    public partial class RentedGetOKResponseContent
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

    public partial class AvailableGetOKResponseContent
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

    public partial class SearchPostOKResponseContent
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

    /// <summary>
    /// Multiple Response Types MoviesGetOKResponseContent, MoviesGetBadRequestResponseContent
    /// </summary>
    public partial class MultipleMoviesGet : ApiMultipleResponse
    {

        static readonly Dictionary<HttpStatusCode, string> schemas = new Dictionary<HttpStatusCode, string>
        {
            { HttpStatusCode.OK, "{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"array\",  \"description\": \"movies\",  \"items\":     {      \"type\": \"object\",        \"properties\": {        \"id\": { \"type\": \"integer\" },        \"name\": { \"type\": \"string\"},        \"director\": { \"type\": \"string\"},        \"genre\": { \"type\": [\"string\", \"null\"] },        \"cast\":{ \"type\": \"string\" },        \"duration\":{ \"type\": [\"number\", \"null\"] },        \"storyline\":{ \"type\": \"string\" },        \"language\":{ \"type\": \"string\" },        \"rented\":{ \"type\": \"boolean\" }    }  }}"},
            { HttpStatusCode.BadRequest, "{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"object\",  \"description\": \"bad request error\",  \"properties\":     {      \"error\": { \"type\": \"string\"},      \"code\": { \"type\": \"integer\" }    }  }"},
        };

        public static string GetSchema(HttpStatusCode statusCode)
        {
            return schemas.ContainsKey(statusCode) ? schemas[statusCode] : string.Empty;
        }

        public MultipleMoviesGet()
        {
            names.Add(HttpStatusCode.OK, "MoviesGetOKResponseContent");
            types.Add(HttpStatusCode.OK, typeof(MoviesGetOKResponseContent[]));
            names.Add(HttpStatusCode.BadRequest, "MoviesGetBadRequestResponseContent");
            types.Add(HttpStatusCode.BadRequest, typeof(MoviesGetBadRequestResponseContent));
        }
        public MoviesGetOKResponseContent[] MoviesGetOKResponseContent { get; set; }
        public MoviesGetBadRequestResponseContent MoviesGetBadRequestResponseContent { get; set; }

    } // end class

    /// <summary>
    /// Multiple Response Types IdGetOKResponseContent, IdGetBadRequestResponseContent, IdGetNotFoundResponseContent
    /// </summary>
    public partial class MultipleIdGet : ApiMultipleResponse
    {

        static readonly Dictionary<HttpStatusCode, string> schemas = new Dictionary<HttpStatusCode, string>
        {
            { HttpStatusCode.OK, "{   \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"object\",  \"description\": \"a movie\",  \"properties\": {    \"id\": { \"type\": \"integer\"},    \"name\": { \"type\": \"string\"},    \"director\": { \"type\": \"string\"},    \"genre\": { \"type\": \"string\" },    \"cast\":{ \"type\": \"string\" },    \"duration\":{ \"type\": \"number\" },    \"storyline\":{ \"type\": \"string\" },    \"language\":{ \"type\": \"string\" },    \"rented\":{ \"type\": \"boolean\" }  }}"},
            { HttpStatusCode.BadRequest, "{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"object\",  \"description\": \"bad request error\",  \"properties\":     {      \"error\": { \"type\": \"string\"},      \"code\": { \"type\": \"integer\" }    }  }"},
            { HttpStatusCode.NotFound, "{ \"$schema\": \"http://json-schema.org/draft-03/schema\",  \"type\": \"object\",  \"description\": \"not found\",  \"properties\":     {      \"id\": { \"type\": \"integer\" },      \"error\": { \"type\": \"string\"},      \"code\": { \"type\": \"integer\" }    }  }"},
        };

        public static string GetSchema(HttpStatusCode statusCode)
        {
            return schemas.ContainsKey(statusCode) ? schemas[statusCode] : string.Empty;
        }

        public MultipleIdGet()
        {
            names.Add(HttpStatusCode.OK, "IdGetOKResponseContent");
            types.Add(HttpStatusCode.OK, typeof(IdGetOKResponseContent));
            names.Add(HttpStatusCode.BadRequest, "IdGetBadRequestResponseContent");
            types.Add(HttpStatusCode.BadRequest, typeof(IdGetBadRequestResponseContent));
            names.Add(HttpStatusCode.NotFound, "IdGetNotFoundResponseContent");
            types.Add(HttpStatusCode.NotFound, typeof(IdGetNotFoundResponseContent));
        }
        public IdGetOKResponseContent IdGetOKResponseContent { get; set; }
        public IdGetBadRequestResponseContent IdGetBadRequestResponseContent { get; set; }
        public IdGetNotFoundResponseContent IdGetNotFoundResponseContent { get; set; }

    } // end class

    public partial class PostSearchQuery
    {
        /// <summary>
        /// Name of the movie
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Director of the movie
        /// </summary>
        public string Director { get; set; }

    } // end class

    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class MoviesIdUriParameters
    {
        public string Id { get; set; }

    } // end class

    /// <summary>
    /// Uri Parameters for resource /rent
    /// </summary>
    public partial class MoviesIdRentUriParameters
    {
        public string Id { get; set; }

    } // end class

    /// <summary>
    /// Uri Parameters for resource /return
    /// </summary>
    public partial class MoviesIdReturnUriParameters
    {
        public string Id { get; set; }

    } // end class

    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class MoviesWishlistIdUriParameters
    {
        public string Id { get; set; }

    } // end class

    public partial class GetMoviesHeader : ApiHeader
    {

        public string User { get; set; }
        public int? Code { get; set; }

    } // end class

    public partial class GetByIdMoviesOKResponseHeader : ApiResponseHeader
    {
        public int? Code { get; set; }
        public DateTime? Created { get; set; }

    } // end class

    public partial class GetByIdMoviesBadRequestResponseHeader : ApiResponseHeader
    {
        public int? Code { get; set; }

    } // end class

    /// <summary>
    /// Multiple Header Types GetByIdMoviesOKResponseHeader, GetByIdMoviesBadRequestResponseHeader
    /// </summary>
    public partial class MultipleGetByIdHeader : ApiMultipleObject
    {

        public MultipleGetByIdHeader()
        {
            names.Add(HttpStatusCode.OK, "GetByIdMoviesOKResponseHeader");
            types.Add(HttpStatusCode.OK, typeof(GetByIdMoviesOKResponseHeader));
            names.Add(HttpStatusCode.BadRequest, "GetByIdMoviesBadRequestResponseHeader");
            types.Add(HttpStatusCode.BadRequest, typeof(GetByIdMoviesBadRequestResponseHeader));
        }

        public void SetProperties(HttpResponseHeaders headers, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                var header = new GetByIdMoviesOKResponseHeader();
                foreach (var responseHeader in headers)
                {
                    var prop = header.GetType().GetProperties().FirstOrDefault(p => p.Name == NetNamingMapper.GetPropertyName(responseHeader.Key));
                    if (prop != null)
                        prop.SetValue(header, responseHeader.Value);
                }
                this.GetByIdMoviesOKResponseHeader = header;
                return;
            }
            if (statusCode == HttpStatusCode.BadRequest)
            {
                var header = new GetByIdMoviesBadRequestResponseHeader();
                foreach (var responseHeader in headers)
                {
                    var prop = header.GetType().GetProperties().FirstOrDefault(p => p.Name == NetNamingMapper.GetPropertyName(responseHeader.Key));
                    if (prop != null)
                        prop.SetValue(header, responseHeader.Value);
                }
                this.GetByIdMoviesBadRequestResponseHeader = header;
                return;
            }
        }
        public GetByIdMoviesOKResponseHeader GetByIdMoviesOKResponseHeader { get; set; }
        public GetByIdMoviesBadRequestResponseHeader GetByIdMoviesBadRequestResponseHeader { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class Rent
    /// </summary>
    public partial class RentPutRequest : ApiRequest
    {
        public RentPutRequest(MoviesIdRentUriParameters UriParameters, HttpContent Content = null, MediaTypeFormatter Formatter = null)
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
    /// Request object for method Put of class Return
    /// </summary>
    public partial class ReturnPutRequest : ApiRequest
    {
        public ReturnPutRequest(MoviesIdReturnUriParameters UriParameters, HttpContent Content = null, MediaTypeFormatter Formatter = null)
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
    /// Request object for method Post of class Wishlist
    /// </summary>
    public partial class WishlistPostRequest : ApiRequest
    {
        public WishlistPostRequest(MoviesWishlistIdUriParameters UriParameters, HttpContent Content = null, MediaTypeFormatter Formatter = null)
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
    /// Request object for method Delete of class Wishlist
    /// </summary>
    public partial class WishlistDeleteRequest : ApiRequest
    {
        public WishlistDeleteRequest(MoviesWishlistIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesWishlistIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Get of class Movies
    /// </summary>
    public partial class MoviesGetRequest : ApiRequest
    {
        public MoviesGetRequest(GetMoviesHeader Headers = null)
        {
            this.Headers = Headers;
        }

        /// <summary>
        /// Typed Request headers
        /// </summary>
        public GetMoviesHeader Headers { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class Movies
    /// </summary>
    public partial class MoviesPostRequest : ApiRequest
    {
        public MoviesPostRequest(MoviesPostRequestContent Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }

        /// <summary>
        /// Request content
        /// </summary>
        public MoviesPostRequestContent Content { get; set; }
        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Request object for method GetById of class Movies
    /// </summary>
    public partial class MoviesGetByIdRequest : ApiRequest
    {
        public MoviesGetByIdRequest(MoviesIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class Movies
    /// </summary>
    public partial class MoviesPutRequest : ApiRequest
    {
        public MoviesPutRequest(MoviesIdUriParameters UriParameters, IdPutRequestContent Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }

        /// <summary>
        /// Request content
        /// </summary>
        public IdPutRequestContent Content { get; set; }
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
    /// Request object for method Delete of class Movies
    /// </summary>
    public partial class MoviesDeleteRequest : ApiRequest
    {
        public MoviesDeleteRequest(MoviesIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public MoviesIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class Search
    /// </summary>
    public partial class SearchPostRequest : ApiRequest
    {
        public SearchPostRequest(HttpContent Content = null, MediaTypeFormatter Formatter = null, PostSearchQuery Query = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.Query = Query;
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
        /// Request query string properties
        /// </summary>
        public PostSearchQuery Query { get; set; }

    } // end class

    /// <summary>
    /// Response object for method Get of class Wishlist
    /// </summary>

    public partial class WishlistGetResponse : ApiResponse
    {


        private WishlistGetOKResponseContent[] typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public WishlistGetOKResponseContent[] Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                var task = Formatters != null && Formatters.Any()
                            ? RawContent.ReadAsAsync<WishlistGetOKResponseContent[]>(Formatters)
                            : RawContent.ReadAsAsync<WishlistGetOKResponseContent[]>();
                task.Wait();
                typedContent = task.GetAwaiter().GetResult();
                return typedContent;
            }
        }

    } // end class

    /// <summary>
    /// Response object for method Get of class Rented
    /// </summary>

    public partial class RentedGetResponse : ApiResponse
    {


        private RentedGetOKResponseContent[] typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public RentedGetOKResponseContent[] Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                var task = Formatters != null && Formatters.Any()
                            ? RawContent.ReadAsAsync<RentedGetOKResponseContent[]>(Formatters)
                            : RawContent.ReadAsAsync<RentedGetOKResponseContent[]>();
                task.Wait();
                typedContent = task.GetAwaiter().GetResult();
                return typedContent;
            }
        }

    } // end class

    /// <summary>
    /// Response object for method Get of class Available
    /// </summary>

    public partial class AvailableGetResponse : ApiResponse
    {


        private AvailableGetOKResponseContent[] typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public AvailableGetOKResponseContent[] Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                var task = Formatters != null && Formatters.Any()
                            ? RawContent.ReadAsAsync<AvailableGetOKResponseContent[]>(Formatters)
                            : RawContent.ReadAsAsync<AvailableGetOKResponseContent[]>();
                task.Wait();
                typedContent = task.GetAwaiter().GetResult();
                return typedContent;
            }
        }

    } // end class

    /// <summary>
    /// Response object for method Get of class Movies
    /// </summary>

    public partial class MoviesGetResponse : ApiResponse
    {

        private MultipleMoviesGet typedContent;
        /// <summary>
        /// Typed response content
        /// </summary>
        public MultipleMoviesGet Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                typedContent = new MultipleMoviesGet();
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
            return MultipleMoviesGet.GetSchema(statusCode);
        }


    } // end class

    /// <summary>
    /// Response object for method GetById of class Movies
    /// </summary>

    public partial class MoviesGetByIdResponse : ApiResponse
    {

        /// <summary>
        /// Typed Response headers (defined in RAML)
        /// </summary>
        public Models.MultipleGetByIdHeader Headers { get; set; }
        private MultipleIdGet typedContent;
        /// <summary>
        /// Typed response content
        /// </summary>
        public MultipleIdGet Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                typedContent = new MultipleIdGet();
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
            return MultipleIdGet.GetSchema(statusCode);
        }


    } // end class

    /// <summary>
    /// Response object for method Post of class Search
    /// </summary>

    public partial class SearchPostResponse : ApiResponse
    {


        private SearchPostOKResponseContent[] typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public SearchPostOKResponseContent[] Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                var task = Formatters != null && Formatters.Any()
                            ? RawContent.ReadAsAsync<SearchPostOKResponseContent[]>(Formatters)
                            : RawContent.ReadAsAsync<SearchPostOKResponseContent[]>();
                task.Wait();
                typedContent = task.GetAwaiter().GetResult();
                return typedContent;
            }
        }

    } // end class


} // end Models namespace