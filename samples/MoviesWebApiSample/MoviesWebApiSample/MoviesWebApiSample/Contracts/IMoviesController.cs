// Template: Controller Interface (ApiControllerInterface.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviesWebApiSample.MoviesV1.Models;


namespace MoviesWebApiSample.MoviesV1
{
    public interface IMoviesController
    {

        IHttpActionResult Get();
        IHttpActionResult Post([FromBody] IList<Movie> iListMovie,[FromUri] string access_token);
        IHttpActionResult GetById([FromUri] string id);
        IHttpActionResult Put(Models.Movie movie,[FromUri] string id);
        IHttpActionResult Delete([FromUri] string id);
        IHttpActionResult PutByIdRent([FromBody] string content,[FromUri] string id,[FromUri] string access_token);
        IHttpActionResult PutByIdReturn([FromBody] string content,[FromUri] string id,[FromUri] string access_token);
        IHttpActionResult GetWishlist([FromUri] string access_token);
        IHttpActionResult PostWishlistById([FromBody] string content,[FromUri] string id,[FromUri] string access_token);
        IHttpActionResult DeleteWishlistById([FromUri] string id,[FromUri] string access_token);
        IHttpActionResult GetRented();
        IHttpActionResult GetAvailable();
    }
}
