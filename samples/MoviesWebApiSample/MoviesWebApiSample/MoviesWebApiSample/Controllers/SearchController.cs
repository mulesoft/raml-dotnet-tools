// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviesWebApiSample.MoviesV1.Models;

namespace MoviesWebApiSample.MoviesV1
{
    public partial class SearchController : ISearchController
    {

/// <summary>
		/// search movies by name or director - /search
		/// </summary>
		/// <param name="name">Name of the movie</param>
		/// <param name="director">Director of the movie</param>
		/// <returns>IList&lt;Movie&gt;</returns>
        public IHttpActionResult Get([FromUri] string name,[FromUri] string director)
        {
            // TODO: implement Get - route: search/search
			// var result = new IList<Movie>();
			// return Ok(result);
			return Ok();
        }

    }
}
