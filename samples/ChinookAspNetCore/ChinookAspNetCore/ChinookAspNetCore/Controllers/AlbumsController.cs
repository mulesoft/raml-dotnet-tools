// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChinookAspNetCore.ChinookV1.Models;

namespace ChinookAspNetCore.ChinookV1
{
    public partial class AlbumsController : IAlbumsController
    {

/// <summary>
		/// /albums
		/// </summary>
		/// <returns>IList&lt;Album&gt;</returns>
        public IActionResult Get()
        {
            // TODO: implement Get - route: albums/albums
			// var result = new IList<Album>();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

/// <summary>
		/// /albums
		/// </summary>
		/// <param name="album"></param>
        public IActionResult Post([FromBody] Models.Album album)
        {
            // TODO: implement Post - route: albums/albums
			return new ObjectResult("");
        }

/// <summary>
		/// /albums/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Album</returns>
        public IActionResult GetById(string id)
        {
            // TODO: implement GetById - route: albums/{id}
			// var result = new Album();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

/// <summary>
		/// /albums/{id}
		/// </summary>
		/// <param name="album"></param>
		/// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Album album,string id)
        {
            // TODO: implement Put - route: albums/{id}
			return new ObjectResult("");
        }

/// <summary>
		/// /albums/{id}
		/// </summary>
		/// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            // TODO: implement Delete - route: albums/{id}
			return new ObjectResult("");
        }

    }
}
