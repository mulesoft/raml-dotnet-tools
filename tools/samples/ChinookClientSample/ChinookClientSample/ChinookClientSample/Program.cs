using System.Linq;
using ChinookClientSample.ChinookV1;
using System.Threading.Tasks;
using ChinookClientSample.ChinookV1.Models;

namespace ChinookClientSample
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

            var response = await client.Artists.GetById(artists.First().Id.ToString());
            var artist = response.Content;

            artist.Name = "Updated";
            await client.Artists.Put(artist, artist.Id.ToString());

            var notFound = await client.Artists.Delete("999999999");
            var notFoundStatus = notFound.StatusCode;

            var deleted = await client.Artists.Delete(artist.Id.ToString());
            var ok = deleted.StatusCode;
        }
    }
}
