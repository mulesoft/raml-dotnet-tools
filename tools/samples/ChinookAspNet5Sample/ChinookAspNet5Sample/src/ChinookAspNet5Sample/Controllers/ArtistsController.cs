// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ChinookAspNet5Sample.ChinookV1.Models;

namespace ChinookAspNet5Sample.ChinookV1
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
                return new HttpNotFoundObjectResult("");

            return new ObjectResult(artists[id]);
        }

        /// <param name="content"></param>
        /// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Artist content, string id)
        {
            if (!artists.ContainsKey(id))
                return new HttpNotFoundObjectResult("");

            artists[id] = content;

            return new ObjectResult(content);
        }

        /// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            if (!artists.ContainsKey(id))
                return new HttpNotFoundObjectResult("");

            artists.Remove(id);
            return new ObjectResult("");
        }

        /// <param name="id"></param>
        /// <returns>IDictionary&lt;string,Artist&gt;</returns>
        public IActionResult GetA(string id)
        {
            // TODO: implement GetA - route: artists/bytrack/{id}
            // var result = new IDictionary<string,Artist>();
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
