// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ChinookAspNetCore.ChinookV1.Models;

namespace ChinookAspNetCore.ChinookV1
{
    public partial class ArtistsController : IArtistsController
    {

        /// <returns>IList&lt;Artist&gt;</returns>
        public IActionResult Get()
        {
            return new ObjectResult(artists.Values);
        }

        /// <param name="content"></param>
        public IActionResult Post([FromBody] Models.Artist content)
        {
            var key = artists.Count + 1;
            artists.Add(key.ToString(), content);
            return new CreatedResult("artists/" + key, artists.Keys);
        }

        /// <param name="id"></param>
        /// <returns>Artist</returns>
        public IActionResult GetById(string id)
        {
            if (!artists.ContainsKey(id))
                return new NotFoundObjectResult("");

            return new ObjectResult(artists[id]);
        }

        /// <param name="content"></param>
        /// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Artist content, string id)
        {
            if (!artists.ContainsKey(id))
                return new NotFoundObjectResult("");

            artists[id] = content;

            return new ObjectResult(content);
        }

        /// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            if (!artists.ContainsKey(id))
                return new NotFoundObjectResult("");

            artists.Remove(id);
            return new ObjectResult("");
        }

        /// <summary>
		/// /artists/bytrack/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns>ArtistByTrack</returns>
        public IActionResult GetBytrackById(string id)
        {
            // TODO: implement GetBytrackById - route: artists/bytrack/{id}
			// var result = new ArtistByTrack();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

        private static readonly IDictionary<string, Artist> artists = new Dictionary<string, Artist>
        {
            {
                "1",
                new Artist
                {
                    Id = 1,
                    Name = "Dave Mathwes Band"
                }
            },
            {
                "2",
                new Artist
                {
                    Id = 2,
                    Name = "Led Zeppelin"
                }
            }
        };
    }
}
