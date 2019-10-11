using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using MoviesClientSample.Movies;
using MoviesClientSample.Movies.Models;

namespace MoviesClientSample
{
    public partial class MainForm : Form
    {
        private MoviesClient client;
	    private readonly string oAuthAccessToken = "asdasdewr34r4wef34r34";

	    public MainForm()
        {
            InitializeComponent();
        }

		private async void Form1_Load(object sender, EventArgs e)
		{
			var movies = await GetMovies();
			dataGridView1.SelectionChanged += DataGridView1OnSelectionChanged;
			dataGridView1.DataSource = movies;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
		}

		public async Task<IList<MoviesGetOKResponseContent>> GetMovies()
	    {
			var fakeResponseHandler = new FakeResponseHandler();
			var httpClient = new HttpClient(fakeResponseHandler) {BaseAddress = new Uri("http://test.com/api/")};

		    client = new MoviesClient(httpClient);
		    client.SchemaValidation.RaiseExceptions = false;
		    var movies = await client.Movies.Get();
		    return movies.Content;
	    }

	    private void DataGridView1OnSelectionChanged(object sender, EventArgs eventArgs)
	    {
		    if (dataGridView1.CurrentRow == null) return;
			var item = (MoviesGetOKResponseContent)dataGridView1.CurrentRow.DataBoundItem;

		    cmdAddToWishlist.Enabled = true;
		    cmdRent.Enabled = !item.Rented.HasValue || !item.Rented.Value;
			cmdReturn.Enabled = item.Rented.HasValue && item.Rented.Value;
	    }

		private async void cmdRent_Click(object sender, EventArgs e)
		{
            var item = (MoviesGetOKResponseContent)dataGridView1.CurrentRow.DataBoundItem;
			item.Rented = true;
			client.OAuthAccessToken = oAuthAccessToken;
			await client.Movies.Rent.Put("", item.Id.ToString());
			dataGridView1.CurrentRow.Cells[8].Value = true;
			dataGridView1.Refresh();
			cmdRent.Enabled = false;
			cmdReturn.Enabled = true;
		}

		private async void cmdReturn_Click(object sender, EventArgs e)
		{
            var item = (MoviesGetOKResponseContent)dataGridView1.CurrentRow.DataBoundItem;
			item.Rented = false;
			client.OAuthAccessToken = oAuthAccessToken;
			await client.Movies.Return.Put("", item.Id.ToString());

			dataGridView1.CurrentRow.Cells[8].Value = false;
			dataGridView1.Refresh();
			cmdRent.Enabled = true;
			cmdReturn.Enabled = false;
		}

		private void cmdWishlist_Click(object sender, EventArgs e)
		{
			var frm = new WishlistForm();
			frm.ShowDialog(this);
		}

		private void cmdRented_Click(object sender, EventArgs e)
		{
			var frm = new RentedForm();
			frm.ShowDialog(this);
		}

		private async void cmdAddToWishlist_Click(object sender, EventArgs e)
		{
            var item = (MoviesGetOKResponseContent)dataGridView1.CurrentRow.DataBoundItem;
			item.Rented = false;
			client.OAuthAccessToken = oAuthAccessToken;
			await client.Movies.Wishlist.Post("", item.Id.ToString());
		}
    }
}
