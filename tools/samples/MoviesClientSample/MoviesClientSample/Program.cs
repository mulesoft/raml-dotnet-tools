using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MoviesClientSample.Movies.Models;
using Newtonsoft.Json;

namespace MoviesClientSample
{
    static class Program
    {
	    /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			BuildRepository();

            Application.Run(new MainForm());
        }

	    private static void BuildRepository()
	    {
            const string json = "[               {      \"id\" : 1,            \"name\": \"Big Fish\",                  \"director\": \"Tim Burton\",                  \"genre\": \"Drama, Fantasy\",                  \"cast\": \"Ewan McGregor, Albert Finney, Billy Crudup\",                  \"language\": \"English\",  \"duration\": 2, \"storyline\": \"the movies is about...\" , \"rented\": false   },              {       \"id\" : 2,           \"name\": \"Midnight in Paris\",                  \"director\": \"Woody Allen\",                  \"genre\": \"Comedy\",                  \"cast\": \"Owen Wilson, Rachel McAdams, Kathy Bates\",                  \"language\": \"English\",  \"duration\": 2, \"storyline\": \"the movies is about...\"  , \"rented\": false             },               {       \"id\" : 3,           \"name\": \"The Matrix\",                  \"director\": \"The Wachowski Brothers\",                  \"genre\": \"Science Fiction, Action\",                  \"cast\": \"Keanu Reeves, Laurence Fishburne, Carrie-Anne Moss\",                  \"language\": \"English\"   , \"rented\": true,  \"duration\": 2, \"storyline\": \"the movies is about...\"            },   {       \"id\" : 4,           \"name\": \"The Shawshank Redemption\",                  \"director\": \"Frank Darabont\",                  \"genre\": \"Crime, Drama\",                  \"cast\": \"  Tim Robbins, Morgan Freeman, Bob Gunton\",                  \"language\": \"English\",  \"duration\": 2, \"storyline\": \"the movies is about...\"  , \"rented\": false             }            ]";
		    var movies = JsonConvert.DeserializeObject<MoviesGetOKResponseContent[]>(json);
		    Repositories.Movies = movies;
			Repositories.Wishlist = new List<MoviesGetOKResponseContent>();
			Repositories.Wishlist.Add(movies.First());
	    }
    }
}
