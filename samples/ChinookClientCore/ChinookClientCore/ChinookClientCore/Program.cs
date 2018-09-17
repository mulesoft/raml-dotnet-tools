using ChinookClientCore.ChinookV1;
using ChinookClientCore.ChinookV1.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookClientCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Test().Wait();
        }

        private static async Task Test()
        {
            var client = new ChinookV1Client("http://localhost:21911/");

            var theBeatles = new Artist { Id = 234, Name = "The Beatles" };
            await client.Artists.Post(theBeatles);

            var resp = await client.Artists.Get();
            var artists = resp.Content;

            var response = await client.ArtistsId.Get(artists.First().Id.ToString());
            var artist = response.Content;

            artist.Name = "Updated";
            await client.ArtistsId.Put(artist, artist.Id.ToString());

            var notFound = await client.ArtistsId.Delete("999999999");
            var notFoundStatus = notFound.StatusCode;

            var deleted = await client.ArtistsId.Delete(artist.Id.ToString());
            var ok = deleted.StatusCode;
        }
    }
}
