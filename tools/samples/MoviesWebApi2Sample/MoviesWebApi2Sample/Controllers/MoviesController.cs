// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviesWebApi2Sample.Movies.Models;

namespace MoviesWebApi2Sample.Movies
{
    public partial class MoviesController : IMoviesController
    {

		/// <summary>
		/// gets all movies in the catalogue
		/// </summary>
		/// <returns>IList&lt;MoviesGetOKResponseContent&gt;</returns>
        public IHttpActionResult Get()
        {
            // TODO: implement Get - route: movies/
			// var result = new IList<MoviesGetOKResponseContent>();
			// return Ok(result);
			return Ok();
        }

		/// <summary>
		/// adds a movie to the catalogue
		/// </summary>
		/// <param name="moviespostrequestcontent"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult Post(Models.MoviesPostRequestContent moviespostrequestcontent,[FromUri] string access_token = null)
        {
            // TODO: implement Post - route: movies/
			return Ok();
        }

		/// <summary>
		/// get the info of a movie
		/// </summary>
		/// <param name="id"></param>
		/// <returns>MoviesIdGetOKResponseContent</returns>
        public IHttpActionResult GetById([FromUri] string id)
        {
            // TODO: implement GetById - route: movies/{id}
			// var result = new MoviesIdGetOKResponseContent();
			// return Ok(result);
			return Ok();
        }

		/// <summary>
		/// update the info of a movie
		/// </summary>
		/// <param name="moviesidputrequestcontent"></param>
		/// <param name="id"></param>
        public IHttpActionResult Put(Models.MoviesIdPutRequestContent moviesidputrequestcontent,[FromUri] string id)
        {
            // TODO: implement Put - route: movies/{id}
			return Ok();
        }

		/// <summary>
		/// remove a movie from the catalogue
		/// </summary>
		/// <param name="id"></param>
        public IHttpActionResult Delete([FromUri] string id)
        {
            // TODO: implement Delete - route: movies/{id}
			return Ok();
        }

		/// <summary>
		/// rent a movie
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult PutRent([FromBody] string content,[FromUri] string id,[FromUri] string access_token = null)
        {
            // TODO: implement PutRent - route: movies/{id}/rent
			return Ok();
        }

		/// <summary>
		/// return a movie
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult PutReturn([FromBody] string content,[FromUri] string id,[FromUri] string access_token = null)
        {
            // TODO: implement PutReturn - route: movies/{id}/return
			return Ok();
        }

		/// <summary>
		/// gets the current user movies wishlist
		/// </summary>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
		/// <returns>IList&lt;MoviesWishlistGetOKResponseContent&gt;</returns>
        public IHttpActionResult GetWishlist([FromUri] string access_token = null)
        {
            // TODO: implement GetWishlist - route: movies/wishlist
			// var result = new IList<MoviesWishlistGetOKResponseContent>();
			// return Ok(result);
			return Ok();
        }

		/// <summary>
		/// add a movie to the current user movies wishlist
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult PostById([FromBody] string content,[FromUri] string id,[FromUri] string access_token = null)
        {
            // TODO: implement PostById - route: movies/wishlist/{id}
			return Ok();
        }

		/// <summary>
		/// removes a movie from the current user movies wishlist
		/// </summary>
		/// <param name="id"></param>
		/// <param name="access_token">Used to send a valid OAuth 2 access token. Do not use together with the &quot;Authorization&quot; header </param>
        public IHttpActionResult DeleteById([FromUri] string id,[FromUri] string access_token = null)
        {
            // TODO: implement DeleteById - route: movies/wishlist/{id}
			return Ok();
        }

		/// <summary>
		/// gets the user rented movies
		/// </summary>
		/// <returns>IList&lt;MoviesRentedGetOKResponseContent&gt;</returns>
        public IHttpActionResult GetRented()
        {
            // TODO: implement GetRented - route: movies/rented
			// var result = new IList<MoviesRentedGetOKResponseContent>();
			// return Ok(result);
			return Ok();
        }

		/// <summary>
		/// get all movies that are not currently rented
		/// </summary>
		/// <returns>IList&lt;MoviesAvailableGetOKResponseContent&gt;</returns>
        public IHttpActionResult GetAvailable()
        {
            // TODO: implement GetAvailable - route: movies/available
			// var result = new IList<MoviesAvailableGetOKResponseContent>();
			// return Ok(result);
			return Ok();
        }

    }
}
