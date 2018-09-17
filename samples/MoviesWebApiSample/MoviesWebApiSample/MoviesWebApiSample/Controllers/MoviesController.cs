// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviesWebApiSample.MoviesV1.Models;

namespace MoviesWebApiSample.MoviesV1
{
    public partial class MoviesController : IMoviesController
    {

/// <summary>
		/// gets all movies in the catalogue - /movies
		/// </summary>
		/// <returns>IList&lt;Movie&gt;</returns>
        public IHttpActionResult Get()
        {
            // TODO: implement Get - route: movies/movies
			// var result = new IList<Movie>();
			// return Ok(result);
			return Ok();
        }

/// <summary>
		/// adds a movie to the catalogue - /movies
		/// </summary>
		/// <param name="iListMovie"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult Post([FromBody] IList<Movie> iListMovie,[FromUri] string access_token)
        {
            // TODO: implement Post - route: movies/movies
			return Ok();
        }

/// <summary>
		/// get the info of a movie - /movies/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Movie</returns>
        public IHttpActionResult GetById([FromUri] string id)
        {
            // TODO: implement GetById - route: movies/{id}
			// var result = new Movie();
			// return Ok(result);
			return Ok();
        }

/// <summary>
		/// update the info of a movie - /movies/{id}
		/// </summary>
		/// <param name="movie"></param>
		/// <param name="id"></param>
        public IHttpActionResult Put(Models.Movie movie,[FromUri] string id)
        {
            // TODO: implement Put - route: movies/{id}
			return Ok();
        }

/// <summary>
		/// remove a movie from the catalogue - /movies/{id}
		/// </summary>
		/// <param name="id"></param>
        public IHttpActionResult Delete([FromUri] string id)
        {
            // TODO: implement Delete - route: movies/{id}
			return Ok();
        }

/// <summary>
		/// rent a movie - /movies/{id}/rent
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult PutByIdRent([FromBody] string content,[FromUri] string id,[FromUri] string access_token)
        {
            // TODO: implement PutByIdRent - route: movies/{id}/rent
			return Ok();
        }

/// <summary>
		/// return a movie - /movies/{id}/return
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult PutByIdReturn([FromBody] string content,[FromUri] string id,[FromUri] string access_token)
        {
            // TODO: implement PutByIdReturn - route: movies/{id}/return
			return Ok();
        }

/// <summary>
		/// gets the current user movies wishlist - /movies/wishlist
		/// </summary>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
		/// <returns>IList&lt;Movie&gt;</returns>
        public IHttpActionResult GetWishlist([FromUri] string access_token)
        {
            // TODO: implement GetWishlist - route: movies/wishlist
			// var result = new IList<Movie>();
			// return Ok(result);
			return Ok();
        }

/// <summary>
		/// add a movie to the current user movies wishlist - /movies/wishlist/{id}
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult PostWishlistById([FromBody] string content,[FromUri] string id,[FromUri] string access_token)
        {
            // TODO: implement PostWishlistById - route: movies/wishlist/{id}
			return Ok();
        }

/// <summary>
		/// removes a movie from the current user movies wishlist - /movies/wishlist/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult DeleteWishlistById([FromUri] string id,[FromUri] string access_token)
        {
            // TODO: implement DeleteWishlistById - route: movies/wishlist/{id}
			return Ok();
        }

/// <summary>
		/// gets the user rented movies - /movies/rented
		/// </summary>
		/// <returns>IList&lt;Movie&gt;</returns>
        public IHttpActionResult GetRented()
        {
            // TODO: implement GetRented - route: movies/rented
			// var result = new IList<Movie>();
			// return Ok(result);
			return Ok();
        }

/// <summary>
		/// get all movies that are not currently rented - /movies/available
		/// </summary>
		/// <returns>IList&lt;Movie&gt;</returns>
        public IHttpActionResult GetAvailable()
        {
            // TODO: implement GetAvailable - route: movies/available
			// var result = new IList<Movie>();
			// return Ok(result);
			return Ok();
        }

    }
}
