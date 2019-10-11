// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ChinookAspNet5Sample.ChinookV1.Models;

namespace ChinookAspNet5Sample.ChinookV1
{
    public partial class AlbumsController : IAlbumsController
    {

		/// <returns>IList&lt;Album&gt;</returns>
        public IActionResult Get()
        {
            // TODO: implement Get - route: albums/
			// var result = new IList<Album>();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

		/// <param name="content"></param>
        public IActionResult Post([FromBody] Models.Album content)
        {
            // TODO: implement Post - route: albums/
			return new ObjectResult("");
        }

		/// <param name="id"></param>
		/// <returns>Album</returns>
        public IActionResult GetById(string id)
        {
            // TODO: implement GetById - route: albums/{id}
			// var result = new Album();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

		/// <param name="content"></param>
		/// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Album content,string id)
        {
            // TODO: implement Put - route: albums/{id}
			return new ObjectResult("");
        }

		/// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            // TODO: implement Delete - route: albums/{id}
			return new ObjectResult("");
        }

    }
}
