// Template: Client Proxy T4 Template (RAMLClient.t4) version 5.0

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
using RAML.Api.Core;
using Raml.Common;
using ChinookClientSample.ChinookV1.Models;

namespace ChinookClientSample.ChinookV1
{
    public partial class Customers
    {
        private readonly ChinookV1Client proxy;

        internal Customers(ChinookV1Client proxy)
        {
            this.proxy = proxy;
        }


        
        public virtual async Task<Models.CustomersGetResponse> Get()
        {

            var url = "customers";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.CustomersGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.CustomersGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "customers";

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
            return new Models.CustomersGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
        public virtual async Task<ApiResponse> Post(Models.Customer content)
        {

            var url = "customers";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new ObjectContent(typeof(Models.Customer), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.CustomersPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.CustomersPostRequest request)
        {

            var url = "customers";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

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


        		/// <param name="id"></param>
        public virtual async Task<Models.CustomersGetByIdResponse> GetById(string id)
        {

            var url = "customers/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.CustomersGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">Models.CustomersGetByIdRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.CustomersGetByIdResponse> GetById(Models.CustomersGetByIdRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "customers/{id}";
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
            return new Models.CustomersGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(Models.Customer content, string id)
        {

            var url = "customers/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);
            req.Content = new ObjectContent(typeof(Models.Customer), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.CustomersPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.CustomersPutRequest request)
        {

            var url = "customers/{id}";
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


        		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Delete(string id)
        {

            var url = "customers/{id}";
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

        		/// <param name="request">Models.CustomersDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.CustomersDeleteRequest request)
        {

            var url = "customers/{id}";
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

    public partial class Byartist
    {
        private readonly ChinookV1Client proxy;

        internal Byartist(ChinookV1Client proxy)
        {
            this.proxy = proxy;
        }


        		/// <param name="id"></param>
        public virtual async Task<Models.ByartistGetResponse> Get(string id)
        {

            var url = "tracks/byartist/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.ByartistGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">Models.ByartistGetRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.ByartistGetResponse> Get(Models.ByartistGetRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "tracks/byartist/{id}";
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
            return new Models.ByartistGetResponse  
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

    public partial class Tracks
    {
        private readonly ChinookV1Client proxy;

        internal Tracks(ChinookV1Client proxy)
        {
            this.proxy = proxy;
        }

        public virtual Byartist Byartist
        {
            get { return new Byartist(proxy); }
        }


        
        public virtual async Task<Models.TracksGetResponse> Get()
        {

            var url = "tracks";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.TracksGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.TracksGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "tracks";

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
            return new Models.TracksGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
        public virtual async Task<ApiResponse> Post(Models.Track content)
        {

            var url = "tracks";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new ObjectContent(typeof(Models.Track), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.TracksPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.TracksPostRequest request)
        {

            var url = "tracks";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

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


        		/// <param name="id"></param>
        public virtual async Task<Models.TracksGetByIdResponse> GetById(string id)
        {

            var url = "tracks/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.TracksGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">Models.TracksGetByIdRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.TracksGetByIdResponse> GetById(Models.TracksGetByIdRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "tracks/{id}";
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
            return new Models.TracksGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(Models.Track content, string id)
        {

            var url = "tracks/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);
            req.Content = new ObjectContent(typeof(Models.Track), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.TracksPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.TracksPutRequest request)
        {

            var url = "tracks/{id}";
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


        		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Delete(string id)
        {

            var url = "tracks/{id}";
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

        		/// <param name="request">Models.TracksDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.TracksDeleteRequest request)
        {

            var url = "tracks/{id}";
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

    public partial class Bytrack
    {
        private readonly ChinookV1Client proxy;

        internal Bytrack(ChinookV1Client proxy)
        {
            this.proxy = proxy;
        }


        		/// <param name="id"></param>
        public virtual async Task<Models.BytrackGetResponse> Get(string id)
        {

            var url = "artists/bytrack/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.BytrackGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">Models.BytrackGetRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.BytrackGetResponse> Get(Models.BytrackGetRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "artists/bytrack/{id}";
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
            return new Models.BytrackGetResponse  
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

    public partial class Artists
    {
        private readonly ChinookV1Client proxy;

        internal Artists(ChinookV1Client proxy)
        {
            this.proxy = proxy;
        }

        public virtual Bytrack Bytrack
        {
            get { return new Bytrack(proxy); }
        }


        
        public virtual async Task<Models.ArtistsGetResponse> Get()
        {

            var url = "artists";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.ArtistsGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.ArtistsGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "artists";

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
            return new Models.ArtistsGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
        public virtual async Task<ApiResponse> Post(Models.Artist content)
        {

            var url = "artists";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new ObjectContent(typeof(Models.Artist), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.ArtistsPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.ArtistsPostRequest request)
        {

            var url = "artists";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

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


        		/// <param name="id"></param>
        public virtual async Task<Models.ArtistsGetByIdResponse> GetById(string id)
        {

            var url = "artists/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.ArtistsGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">Models.ArtistsGetByIdRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.ArtistsGetByIdResponse> GetById(Models.ArtistsGetByIdRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "artists/{id}";
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
            return new Models.ArtistsGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(Models.Artist content, string id)
        {

            var url = "artists/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);
            req.Content = new ObjectContent(typeof(Models.Artist), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.ArtistsPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.ArtistsPutRequest request)
        {

            var url = "artists/{id}";
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


        		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Delete(string id)
        {

            var url = "artists/{id}";
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

        		/// <param name="request">Models.ArtistsDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.ArtistsDeleteRequest request)
        {

            var url = "artists/{id}";
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

    public partial class Albums
    {
        private readonly ChinookV1Client proxy;

        internal Albums(ChinookV1Client proxy)
        {
            this.proxy = proxy;
        }


        
        public virtual async Task<Models.AlbumsGetResponse> Get()
        {

            var url = "albums";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.AlbumsGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.AlbumsGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "albums";

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
            return new Models.AlbumsGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
        public virtual async Task<ApiResponse> Post(Models.Album content)
        {

            var url = "albums";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new ObjectContent(typeof(Models.Album), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.AlbumsPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.AlbumsPostRequest request)
        {

            var url = "albums";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

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


        		/// <param name="id"></param>
        public virtual async Task<Models.AlbumsGetByIdResponse> GetById(string id)
        {

            var url = "albums/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.AlbumsGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">Models.AlbumsGetByIdRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.AlbumsGetByIdResponse> GetById(Models.AlbumsGetByIdRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "albums/{id}";
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
            return new Models.AlbumsGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(Models.Album content, string id)
        {

            var url = "albums/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);
            req.Content = new ObjectContent(typeof(Models.Album), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.AlbumsPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.AlbumsPutRequest request)
        {

            var url = "albums/{id}";
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


        		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Delete(string id)
        {

            var url = "albums/{id}";
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

        		/// <param name="request">Models.AlbumsDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.AlbumsDeleteRequest request)
        {

            var url = "albums/{id}";
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

    public partial class Invoices
    {
        private readonly ChinookV1Client proxy;

        internal Invoices(ChinookV1Client proxy)
        {
            this.proxy = proxy;
        }


        
        public virtual async Task<Models.InvoicesGetResponse> Get()
        {

            var url = "invoices";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.InvoicesGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">ApiRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.InvoicesGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "invoices";

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
            return new Models.InvoicesGetResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
        public virtual async Task<ApiResponse> Post(Models.Invoice content)
        {

            var url = "invoices";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new ObjectContent(typeof(Models.Invoice), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.InvoicesPostRequest</param>
        public virtual async Task<ApiResponse> Post(Models.InvoicesPostRequest request)
        {

            var url = "invoices";

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Post, url);

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


        		/// <param name="id"></param>
        public virtual async Task<Models.InvoicesGetByIdResponse> GetById(string id)
        {

            var url = "invoices/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Get, url);
	        var response = await proxy.Client.SendAsync(req);

            return new Models.InvoicesGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers, 
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };

        }

        		/// <param name="request">Models.InvoicesGetByIdRequest</param>
		/// <param name="responseFormatters">response formatters</param>
        public virtual async Task<Models.InvoicesGetByIdResponse> GetById(Models.InvoicesGetByIdRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "invoices/{id}";
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
            return new Models.InvoicesGetByIdResponse  
                                            {
                                                RawContent = response.Content,
                                                RawHeaders = response.Headers,
	                                            Formatters = responseFormatters,
                                                StatusCode = response.StatusCode,
                                                ReasonPhrase = response.ReasonPhrase,
												SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
                                            };
        }


        		/// <param name="content"></param>
		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Put(Models.Invoice content, string id)
        {

            var url = "invoices/{id}";
            url = url.Replace("{id}", id.ToString());

            url = url.Replace("?&", "?");

            var req = new HttpRequestMessage(HttpMethod.Put, url);
            req.Content = new ObjectContent(typeof(Models.Invoice), content, new JsonMediaTypeFormatter());                           
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

        		/// <param name="request">Models.InvoicesPutRequest</param>
        public virtual async Task<ApiResponse> Put(Models.InvoicesPutRequest request)
        {

            var url = "invoices/{id}";
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


        		/// <param name="id"></param>
        public virtual async Task<ApiResponse> Delete(string id)
        {

            var url = "invoices/{id}";
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

        		/// <param name="request">Models.InvoicesDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.InvoicesDeleteRequest request)
        {

            var url = "invoices/{id}";
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
    /// Main class for grouping root resources. Nested resources are defined as properties. The constructor can optionally receive an URL and HttpClient instance to override the default ones.
    /// </summary>
    public partial class ChinookV1Client
    {

		public SchemaValidationSettings SchemaValidation { get; private set; } 

        protected readonly HttpClient client;
        public const string BaseUri = "";

        internal HttpClient Client { get { return client; } }




        public ChinookV1Client(string endpointUrl)
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

        public ChinookV1Client(HttpClient httpClient)
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

        

        public virtual Customers Customers
        {
            get { return new Customers(this); }
        }
                

        public virtual Tracks Tracks
        {
            get { return new Tracks(this); }
        }
                

        public virtual Artists Artists
        {
            get { return new Artists(this); }
        }
                

        public virtual Albums Albums
        {
            get { return new Albums(this); }
        }
                

        public virtual Invoices Invoices
        {
            get { return new Invoices(this); }
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









namespace ChinookClientSample.ChinookV1.Models
{
    public partial class  Person 
    {

        public int Id { get; set; }


        public string Title { get; set; }


        public string BirthDate { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string Address { get; set; }


        public string City { get; set; }


        public string State { get; set; }


        public string Country { get; set; }


        public string PostalCode { get; set; }


        public string Phone { get; set; }


        public string Fax { get; set; }


        public string Email { get; set; }


        public byte[] Picture { get; set; }


    } // end class

    public partial class Employee  : Person
    {

        public string HireDate { get; set; }


    } // end class

    public partial class ReportsTo  : Employee
    {

    } // end class

    public partial class SupportRepresentant  : Employee
    {

        public ReportsTo ReportsTo { get; set; }


    } // end class

    public partial class Customer  : Person
    {

        public string Company { get; set; }


    } // end class

    public partial class  Artist 
    {

        public int Id { get; set; }


        public string Name { get; set; }


    } // end class

    public partial class  Album 
    {

        public Artist Artist { get; set; }


        public int Id { get; set; }


        public string Title { get; set; }


    } // end class

    public partial class  MediaType 
    {

        public int Id { get; set; }


        public string Name { get; set; }


    } // end class

    public partial class  Genre 
    {

        public int Id { get; set; }


        public string Name { get; set; }


    } // end class

    public partial class  Track 
    {

        public int Id { get; set; }


        public string Name { get; set; }


        public Album Album { get; set; }


        public MediaType MediaType { get; set; }


        public Genre Genre { get; set; }


        public string Composer { get; set; }


        public int Milliseconds { get; set; }


        public int Bytes { get; set; }


        public decimal UnitPrice { get; set; }


    } // end class

    public partial class  InvoiceLine 
    {

        public int Id { get; set; }


        public Track Track { get; set; }


        public decimal UnitPrice { get; set; }


        public int Quantity { get; set; }


    } // end class

    public partial class  Invoice 
    {

        public Customer Customer { get; set; }


        public string InvoiceDate { get; set; }


        public string BillingAddress { get; set; }


        public string BillingCity { get; set; }


        public string BillingState { get; set; }


        public string BillingCountry { get; set; }


        public string BillingPostalCode { get; set; }


        public IList<InvoiceLine> Lines { get; set; }


    } // end class

    public partial class ArtistByTrack  : Dictionary<string,Artist>
    {

    } // end class

    public partial class TracksByArtist  : Dictionary<string,IList<Track>>
    {

    } // end class

    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class  CustomersIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class  TracksIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class  TracksByartistIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class  ArtistsIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class  ArtistsBytrackIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class  AlbumsIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Uri Parameters for resource /{id}
    /// </summary>
    public partial class  InvoicesIdUriParameters 
    {

		[JsonProperty("id")]
        public string Id { get; set; }


    } // end class

    /// <summary>
    /// Request object for method Post of class Customers
    /// </summary>
    public partial class CustomersPostRequest : ApiRequest
    {
        public CustomersPostRequest(Customer Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Customer Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Request object for method GetById of class Customers
    /// </summary>
    public partial class CustomersGetByIdRequest : ApiRequest
    {
        public CustomersGetByIdRequest(CustomersIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public CustomersIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class Customers
    /// </summary>
    public partial class CustomersPutRequest : ApiRequest
    {
        public CustomersPutRequest(CustomersIdUriParameters UriParameters, Customer Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Customer Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public CustomersIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Delete of class Customers
    /// </summary>
    public partial class CustomersDeleteRequest : ApiRequest
    {
        public CustomersDeleteRequest(CustomersIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public CustomersIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Get of class Byartist
    /// </summary>
    public partial class ByartistGetRequest : ApiRequest
    {
        public ByartistGetRequest(TracksByartistIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public TracksByartistIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class Tracks
    /// </summary>
    public partial class TracksPostRequest : ApiRequest
    {
        public TracksPostRequest(Track Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Track Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Request object for method GetById of class Tracks
    /// </summary>
    public partial class TracksGetByIdRequest : ApiRequest
    {
        public TracksGetByIdRequest(TracksIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public TracksIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class Tracks
    /// </summary>
    public partial class TracksPutRequest : ApiRequest
    {
        public TracksPutRequest(TracksIdUriParameters UriParameters, Track Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Track Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public TracksIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Delete of class Tracks
    /// </summary>
    public partial class TracksDeleteRequest : ApiRequest
    {
        public TracksDeleteRequest(TracksIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public TracksIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Get of class Bytrack
    /// </summary>
    public partial class BytrackGetRequest : ApiRequest
    {
        public BytrackGetRequest(ArtistsBytrackIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public ArtistsBytrackIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class Artists
    /// </summary>
    public partial class ArtistsPostRequest : ApiRequest
    {
        public ArtistsPostRequest(Artist Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Artist Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Request object for method GetById of class Artists
    /// </summary>
    public partial class ArtistsGetByIdRequest : ApiRequest
    {
        public ArtistsGetByIdRequest(ArtistsIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public ArtistsIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class Artists
    /// </summary>
    public partial class ArtistsPutRequest : ApiRequest
    {
        public ArtistsPutRequest(ArtistsIdUriParameters UriParameters, Artist Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Artist Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public ArtistsIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Delete of class Artists
    /// </summary>
    public partial class ArtistsDeleteRequest : ApiRequest
    {
        public ArtistsDeleteRequest(ArtistsIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public ArtistsIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class Albums
    /// </summary>
    public partial class AlbumsPostRequest : ApiRequest
    {
        public AlbumsPostRequest(Album Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Album Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Request object for method GetById of class Albums
    /// </summary>
    public partial class AlbumsGetByIdRequest : ApiRequest
    {
        public AlbumsGetByIdRequest(AlbumsIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public AlbumsIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class Albums
    /// </summary>
    public partial class AlbumsPutRequest : ApiRequest
    {
        public AlbumsPutRequest(AlbumsIdUriParameters UriParameters, Album Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Album Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public AlbumsIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Delete of class Albums
    /// </summary>
    public partial class AlbumsDeleteRequest : ApiRequest
    {
        public AlbumsDeleteRequest(AlbumsIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public AlbumsIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class Invoices
    /// </summary>
    public partial class InvoicesPostRequest : ApiRequest
    {
        public InvoicesPostRequest(Invoice Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Invoice Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Request object for method GetById of class Invoices
    /// </summary>
    public partial class InvoicesGetByIdRequest : ApiRequest
    {
        public InvoicesGetByIdRequest(InvoicesIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public InvoicesIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Put of class Invoices
    /// </summary>
    public partial class InvoicesPutRequest : ApiRequest
    {
        public InvoicesPutRequest(InvoicesIdUriParameters UriParameters, Invoice Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request content
        /// </summary>
        public Invoice Content { get; set; }

        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public InvoicesIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Delete of class Invoices
    /// </summary>
    public partial class InvoicesDeleteRequest : ApiRequest
    {
        public InvoicesDeleteRequest(InvoicesIdUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }


        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public InvoicesIdUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Response object for method Get of class Customers
    /// </summary>

    public partial class CustomersGetResponse : ApiResponse
    {


	    private IList<Customer> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Customer> Content 
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
                    typedContent = (IList<Customer>)new XmlSerializer(typeof(IList<Customer>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Customer>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Customer>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method GetById of class Customers
    /// </summary>

    public partial class CustomersGetByIdResponse : ApiResponse
    {


	    private Customer typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public Customer Content 
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
                    typedContent = (Customer)new XmlSerializer(typeof(Customer)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<Customer>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<Customer>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class Byartist
    /// </summary>

    public partial class ByartistGetResponse : ApiResponse
    {


	    private TracksByArtist typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public TracksByArtist Content 
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
                    typedContent = (TracksByArtist)new XmlSerializer(typeof(TracksByArtist)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<TracksByArtist>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<TracksByArtist>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class Tracks
    /// </summary>

    public partial class TracksGetResponse : ApiResponse
    {


	    private IList<Track> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Track> Content 
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
                    typedContent = (IList<Track>)new XmlSerializer(typeof(IList<Track>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Track>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Track>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method GetById of class Tracks
    /// </summary>

    public partial class TracksGetByIdResponse : ApiResponse
    {


	    private Track typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public Track Content 
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
                    typedContent = (Track)new XmlSerializer(typeof(Track)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<Track>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<Track>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class Bytrack
    /// </summary>

    public partial class BytrackGetResponse : ApiResponse
    {


	    private ArtistByTrack typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public ArtistByTrack Content 
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
                    typedContent = (ArtistByTrack)new XmlSerializer(typeof(ArtistByTrack)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<ArtistByTrack>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<ArtistByTrack>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class Artists
    /// </summary>

    public partial class ArtistsGetResponse : ApiResponse
    {


	    private IList<Artist> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Artist> Content 
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
                    typedContent = (IList<Artist>)new XmlSerializer(typeof(IList<Artist>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Artist>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Artist>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method GetById of class Artists
    /// </summary>

    public partial class ArtistsGetByIdResponse : ApiResponse
    {


	    private Artist typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public Artist Content 
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
                    typedContent = (Artist)new XmlSerializer(typeof(Artist)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<Artist>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<Artist>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class Albums
    /// </summary>

    public partial class AlbumsGetResponse : ApiResponse
    {


	    private IList<Album> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Album> Content 
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
                    typedContent = (IList<Album>)new XmlSerializer(typeof(IList<Album>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Album>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Album>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method GetById of class Albums
    /// </summary>

    public partial class AlbumsGetByIdResponse : ApiResponse
    {


	    private Album typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public Album Content 
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
                    typedContent = (Album)new XmlSerializer(typeof(Album)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<Album>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<Album>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method Get of class Invoices
    /// </summary>

    public partial class InvoicesGetResponse : ApiResponse
    {


	    private IList<Invoice> typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public IList<Invoice> Content 
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
                    typedContent = (IList<Invoice>)new XmlSerializer(typeof(IList<Invoice>)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<IList<Invoice>>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<IList<Invoice>>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class

    /// <summary>
    /// Response object for method GetById of class Invoices
    /// </summary>

    public partial class InvoicesGetByIdResponse : ApiResponse
    {


	    private Invoice typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public Invoice Content 
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
                    typedContent = (Invoice)new XmlSerializer(typeof(Invoice)).Deserialize(xmlStream);
                }
                else
                {
                    var task =  Formatters != null && Formatters.Any() 
                                ? RawContent.ReadAsAsync<Invoice>(Formatters).ConfigureAwait(false)
                                : RawContent.ReadAsAsync<Invoice>().ConfigureAwait(false);
		        
		            typedContent = task.GetAwaiter().GetResult();
                }
		        return typedContent;
	        }
	    }

		


    } // end class


} // end Models namespace
