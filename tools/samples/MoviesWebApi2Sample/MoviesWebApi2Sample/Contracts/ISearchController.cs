// Template: Controller Interface (ApiControllerInterface.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviesWebApi2Sample.Movies.Models;


namespace MoviesWebApi2Sample.Movies
{
    public interface ISearchController
    {

        IHttpActionResult Get([FromUri] string name = null,[FromUri] string director = null);
    }
}
