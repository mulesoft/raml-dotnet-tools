using System.Collections.Generic;
using MoviesClientSample.Movies.Models;

namespace MoviesClientSample
{
	public static class Repositories
	{
        public static IList<MoviesGetOKResponseContent> Movies { get; set; }
        public static IList<MoviesGetOKResponseContent> Wishlist { get; set; }
	}
}