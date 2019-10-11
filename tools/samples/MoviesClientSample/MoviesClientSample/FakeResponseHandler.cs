using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MoviesClientSample
{
	public class FakeResponseHandler : DelegatingHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var responseMessage = new HttpResponseMessage();
			var content = "";
			var localPath = request.RequestUri.LocalPath;
			if (localPath.EndsWith("/movies"))
				content = JsonConvert.SerializeObject(Repositories.Movies);

			if (localPath.EndsWith("/available"))
				content = JsonConvert.SerializeObject(Repositories.Movies.Where(m => !m.Rented.HasValue || !m.Rented.Value));

			if (localPath.EndsWith("/wishlist"))
				content = JsonConvert.SerializeObject(Repositories.Wishlist);

			if (localPath.EndsWith("/rented"))
				content = JsonConvert.SerializeObject(Repositories.Movies.Where(m => m.Rented.HasValue && m.Rented.Value));

			if (localPath.EndsWith("/rent"))
			{
				var url = localPath;
				var id = url.Replace("/api/movies/", string.Empty).Replace("/rent", string.Empty);
				var movie = Repositories.Movies.First(m => m.Id == Convert.ToInt32(id));
				movie.Rented = true;
			}

			if (localPath.EndsWith("/return"))
			{
				var url = localPath;
				var id = url.Replace("/api/movies/", string.Empty).Replace("/return", string.Empty);
				var movie = Repositories.Movies.First(m => m.Id == Convert.ToInt32(id));
				movie.Rented = false;
			}

			if (localPath.StartsWith("/api/movies/wishlist/") && !localPath.EndsWith("/wishlist"))
			{
				var url = localPath;
				var id = url.Replace("/api/movies/wishlist/", string.Empty);
				var movie = Repositories.Movies.First(m => m.Id == Convert.ToInt32(id));
				if(!Repositories.Wishlist.Contains(movie))
					Repositories.Wishlist.Add(movie);
			}
			
			responseMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
			return Task.FromResult(responseMessage);
		}
	}
}