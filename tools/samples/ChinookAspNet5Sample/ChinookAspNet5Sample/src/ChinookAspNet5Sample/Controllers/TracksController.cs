// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ChinookAspNet5Sample.ChinookV1.Models;

namespace ChinookAspNet5Sample.ChinookV1
{
    public partial class TracksController : ITracksController
    {

		/// <returns>IList&lt;Track&gt;</returns>
        public IActionResult Get()
        {
            // TODO: implement Get - route: tracks/
			// var result = new IList<Track>();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

		/// <param name="content"></param>
        public IActionResult Post([FromBody] Models.Track content)
        {
            // TODO: implement Post - route: tracks/
			return new ObjectResult("");
        }

		/// <param name="id"></param>
		/// <returns>Track</returns>
        public IActionResult GetById(string id)
        {
            // TODO: implement GetById - route: tracks/{id}
			// var result = new Track();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

		/// <param name="content"></param>
		/// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Track content,string id)
        {
            // TODO: implement Put - route: tracks/{id}
			return new ObjectResult("");
        }

		/// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            // TODO: implement Delete - route: tracks/{id}
			return new ObjectResult("");
        }

		/// <param name="id"></param>
		/// <returns>IDictionary&lt;string,IList&lt;Track&gt;&gt;</returns>
        public IActionResult GetA(string id)
        {
            // TODO: implement GetA - route: tracks/byartist/{id}
			// var result = new IDictionary<string,IList<Track>>();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

    }
}
