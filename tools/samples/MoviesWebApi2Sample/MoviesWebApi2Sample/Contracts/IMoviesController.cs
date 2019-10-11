// Template: Controller Interface (ApiControllerInterface.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviesWebApi2Sample.Movies.Models;


namespace MoviesWebApi2Sample.Movies
{
    public interface IMoviesController
    {

        IHttpActionResult Get();
        IHttpActionResult Post(Models.MoviesPostRequestContent moviespostrequestcontent,[FromUri] string access_token = null);
        IHttpActionResult GetById([FromUri] string id);
        IHttpActionResult Put(Models.MoviesIdPutRequestContent moviesidputrequestcontent,[FromUri] string id);
        IHttpActionResult Delete([FromUri] string id);
        IHttpActionResult PutRent([FromBody] string content,[FromUri] string id,[FromUri] string access_token = null);
        IHttpActionResult PutReturn([FromBody] string content,[FromUri] string id,[FromUri] string access_token = null);
        IHttpActionResult GetWishlist([FromUri] string access_token = null);
        IHttpActionResult PostById([FromBody] string content,[FromUri] string id,[FromUri] string access_token = null);
        IHttpActionResult DeleteById([FromUri] string id,[FromUri] string access_token = null);
        IHttpActionResult GetRented();
        IHttpActionResult GetAvailable();
    }
}
