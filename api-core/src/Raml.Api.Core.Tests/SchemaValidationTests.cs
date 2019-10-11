using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAML.Api.Core;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class SchemaValidationTests
    {
        [TestMethod]
        public async Task ShouldNotValidateSchema()
        {
            var content = new StringContent("{ name: 'foo' }",
                Encoding.UTF8,
                "application/json");

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = content;

            var handler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://localhost");

            var proxy = new Movies.MoviesApi(client);
            proxy.SchemaValidation.RaiseExceptions = false;

            var movies = await proxy.Movies.Get();

            Assert.IsFalse(movies.SchemaValidation.Value.IsValid);
                        
        }

        [TestMethod]
        public async Task ShouldValidateV3Schema()
        {
            var content = new StringContent("[{ id : 1, name: 'Big Fish', director: 'Tim Burton', genre: 'Drama, Fantasy', cast: 'Ewan McGregor, Albert Finney, Billy Crudup', duration: 90, storyline: 'none', language: 'English', rented: false }]",
                Encoding.UTF8,
                "application/json");

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = content;

            var handler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://localhost");

            var proxy = new Movies.MoviesApi(client);
            proxy.SchemaValidation.RaiseExceptions = false;

            var movies = await proxy.Movies.Get();

            Assert.IsTrue(movies.SchemaValidation.Value.IsValid);

        }

        [TestMethod]
        public async Task ShouldValidateNulls()
        {
            var content = new StringContent("[{ id : 1, name: 'Big Fish', director: 'Tim Burton', genre: null, cast: 'Ewan McGregor, Albert Finney, Billy Crudup', duration: null, storyline: 'none', language: 'English', rented: false }]",
                Encoding.UTF8,
                "application/json");

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = content;

            var handler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://localhost");

            var proxy = new Movies.MoviesApi(client);
            proxy.SchemaValidation.RaiseExceptions = false;

            var movies = await proxy.Movies.Get();

            Assert.IsTrue(movies.SchemaValidation.Value.IsValid);

        }

        [TestMethod]
        public async Task ShouldIgnoreNonJsonSchemas()
        {
            var content = new StringContent("content",
                Encoding.UTF8,
                "text/plain");

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = content;

            var handler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://localhost");

            var proxy = new Movies.MoviesApi(client);
            proxy.SchemaValidation.RaiseExceptions = false;

            var movies = await proxy.Movies.Get();

            Assert.IsTrue(movies.SchemaValidation.Value.IsValid);

        }

        [TestMethod]
        [ExpectedException(typeof(SchemaValidationException))]
        public async Task ShouldThrowExceptionForInvalidSchema()
        {
            var content = new StringContent("{ name: 'foo' }",
                Encoding.UTF8,
                "application/json");

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = content;

            var handler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://localhost");

            var proxy = new Movies.MoviesApi(client);
            proxy.SchemaValidation.RaiseExceptions = true;

            var movies = await proxy.Movies.Get();
        }

        [TestMethod]
        public async Task ShouldNotThrowExceptionForValidSchema()
        {
            var content = new StringContent("[{ id : 1, name: 'Big Fish', director: 'Tim Burton', genre: 'Drama, Fantasy', cast: 'Ewan McGregor, Albert Finney, Billy Crudup', duration: 90, storyline: 'none', language: 'English', rented: false }]",
                Encoding.UTF8,
                "application/json");

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = content;

            var handler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://localhost");

            var proxy = new Movies.MoviesApi(client);
            proxy.SchemaValidation.RaiseExceptions = true;

            var movies = await proxy.Movies.Get();

            Assert.IsNotNull(movies);
        }

        [TestMethod]
        public async Task ShouldNotThrowExceptionForNonJSONSchema()
        {
            var content = new StringContent("data",
                Encoding.UTF8,
                "text/plain");

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = content;

            var handler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://localhost");

            var proxy = new Movies.MoviesApi(client);
            proxy.SchemaValidation.RaiseExceptions = true;

            var movies = await proxy.Movies.Get();

            Assert.IsNotNull(movies);
        }

        [TestMethod]
        public async Task ShouldValidateV4Schema()
        {
            var content = new StringContent("{ Component: 'component', Version: 'version' }",
                Encoding.UTF8,
                "application/json");

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = content;

            var handler = new FakeHttpMessageHandler(response);
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://localhost");

            var proxy = new EPiServerServiceAPI.EpiApi(client);
            proxy.SchemaValidation.RaiseExceptions = false;
            proxy.OAuthAccessToken = "token";

            var version = await proxy.Version.Get();

            Assert.IsTrue(version.SchemaValidation.Value.IsValid);

        }

        class FakeHttpMessageHandler : HttpMessageHandler
        {
            HttpResponseMessage response;

            public FakeHttpMessageHandler(HttpResponseMessage response)
            {
                this.response = response;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                System.Threading.CancellationToken cancellationToken)
            {
                var tcs = new TaskCompletionSource<HttpResponseMessage>();

                tcs.SetResult(response);

                return tcs.Task;
            }
        }
    }
}
