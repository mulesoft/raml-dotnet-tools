using System;
using System.Net.Http;
using System.Windows.Forms;
using MoviesClientSample.Movies;

namespace MoviesClientSample
{
	public partial class RentedForm : Form
	{
		private MoviesClient client;

		public RentedForm()
		{
			InitializeComponent();
		}

		private async void RentedForm_Load(object sender, EventArgs e)
		{
			client = new MoviesClient(new HttpClient(new FakeResponseHandler()) {BaseAddress = new Uri("http://test.com/api/")});
			client.OAuthAccessToken = "sadasdasdasdasdasdasd4545345345343asd";
			var rentedMovies = await client.Movies.Rented.Get();
			dataGridView1.DataSource = rentedMovies.Content;
			FormBorderStyle = FormBorderStyle.FixedDialog;
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
