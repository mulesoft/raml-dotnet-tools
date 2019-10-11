using System;
using System.Net.Http;
using System.Windows.Forms;
using MoviesClientSample.Movies;

namespace MoviesClientSample
{
	public partial class WishlistForm : Form
	{
		private MoviesClient client;

		public WishlistForm()
		{
			InitializeComponent();
		}

		private async void WishlistForm_Load(object sender, EventArgs e)
		{
			client = new MoviesClient(new HttpClient(new FakeResponseHandler()) {BaseAddress = new Uri("http://test.com/api/")});
			client.OAuthAccessToken = "sadasdasdasdasdasdasd4545345345343asd";
			var wishlist = await client.Movies.Wishlist.Get();
			dataGridView1.DataSource = wishlist.Content;
			FormBorderStyle = FormBorderStyle.FixedDialog;
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
