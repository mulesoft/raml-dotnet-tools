// Template: Controller Interface (ApiControllerInterface.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviesWebApiSample.MoviesV1.Models;


namespace MoviesWebApiSample.MoviesV1
{
    public interface ISearchController
    {

        IHttpActionResult Get([FromUri] string name,[FromUri] string director);
    }
}
