// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChinookAspNetCore.ChinookV1.Models;

namespace ChinookAspNetCore.ChinookV1
{
    public partial class TracksController : ITracksController
    {

/// <summary>
		/// /tracks
		/// </summary>
		/// <returns>IList&lt;Track&gt;</returns>
        public IActionResult Get()
        {
            // TODO: implement Get - route: tracks/tracks
			// var result = new IList<Track>();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

/// <summary>
		/// /tracks
		/// </summary>
		/// <param name="track"></param>
        public IActionResult Post([FromBody] Models.Track track)
        {
            // TODO: implement Post - route: tracks/tracks
			return new ObjectResult("");
        }

/// <summary>
		/// /tracks/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Track</returns>
        public IActionResult GetById(string id)
        {
            // TODO: implement GetById - route: tracks/{id}
			// var result = new Track();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

/// <summary>
		/// /tracks/{id}
		/// </summary>
		/// <param name="track"></param>
		/// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Track track,string id)
        {
            // TODO: implement Put - route: tracks/{id}
			return new ObjectResult("");
        }

/// <summary>
		/// /tracks/{id}
		/// </summary>
		/// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            // TODO: implement Delete - route: tracks/{id}
			return new ObjectResult("");
        }

/// <summary>
		/// /tracks/byartist/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns>TracksByArtist</returns>
        public IActionResult GetByartistById(string id)
        {
            // TODO: implement GetByartistById - route: tracks/byartist/{id}
			// var result = new TracksByArtist();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

    }
}
