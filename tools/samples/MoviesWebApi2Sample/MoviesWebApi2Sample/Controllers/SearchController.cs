// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviesWebApi2Sample.Movies.Models;

namespace MoviesWebApi2Sample.Movies
{
    public partial class SearchController : ISearchController
    {

		/// <summary>
		/// search movies by name or director
		/// </summary>
		/// <param name="name">Name of the movie</param>
		/// <param name="director">Director of the movie</param>
		/// <returns>IList&lt;SearchGetOKResponseContent&gt;</returns>
        public IHttpActionResult Get([FromUri] string name = null,[FromUri] string director = null)
        {
            // TODO: implement Get - route: search/
			// var result = new IList<SearchGetOKResponseContent>();
			// return Ok(result);
			return Ok();
        }

    }
}
