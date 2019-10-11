namespace MoviesClientSample
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.cmdRent = new System.Windows.Forms.Button();
			this.cmdAddToWishlist = new System.Windows.Forms.Button();
			this.lblMovies = new System.Windows.Forms.Label();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.cmdReturn = new System.Windows.Forms.Button();
			this.cmdWishlist = new System.Windows.Forms.Button();
			this.cmdRented = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdRent
			// 
			this.cmdRent.Enabled = false;
			this.cmdRent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdRent.Location = new System.Drawing.Point(712, 170);
			this.cmdRent.Name = "cmdRent";
			this.cmdRent.Size = new System.Drawing.Size(75, 23);
			this.cmdRent.TabIndex = 2;
			this.cmdRent.Text = "Rent";
			this.cmdRent.UseVisualStyleBackColor = true;
			this.cmdRent.Click += new System.EventHandler(this.cmdRent_Click);
			// 
			// cmdAddToWishlist
			// 
			this.cmdAddToWishlist.Enabled = false;
			this.cmdAddToWishlist.Location = new System.Drawing.Point(874, 170);
			this.cmdAddToWishlist.Name = "cmdAddToWishlist";
			this.cmdAddToWishlist.Size = new System.Drawing.Size(101, 23);
			this.cmdAddToWishlist.TabIndex = 3;
			this.cmdAddToWishlist.Text = "Add to Wishlist";
			this.cmdAddToWishlist.UseVisualStyleBackColor = true;
			this.cmdAddToWishlist.Click += new System.EventHandler(this.cmdAddToWishlist_Click);
			// 
			// lblMovies
			// 
			this.lblMovies.AutoSize = true;
			this.lblMovies.Location = new System.Drawing.Point(3, 6);
			this.lblMovies.Name = "lblMovies";
			this.lblMovies.Size = new System.Drawing.Size(41, 13);
			this.lblMovies.TabIndex = 4;
			this.lblMovies.Text = "Movies";
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(6, 22);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.Size = new System.Drawing.Size(969, 142);
			this.dataGridView1.TabIndex = 5;
			// 
			// cmdReturn
			// 
			this.cmdReturn.Location = new System.Drawing.Point(793, 170);
			this.cmdReturn.Name = "cmdReturn";
			this.cmdReturn.Size = new System.Drawing.Size(75, 23);
			this.cmdReturn.TabIndex = 6;
			this.cmdReturn.Text = "Return";
			this.cmdReturn.UseVisualStyleBackColor = true;
			this.cmdReturn.Click += new System.EventHandler(this.cmdReturn_Click);
			// 
			// cmdWishlist
			// 
			this.cmdWishlist.Location = new System.Drawing.Point(6, 170);
			this.cmdWishlist.Name = "cmdWishlist";
			this.cmdWishlist.Size = new System.Drawing.Size(75, 23);
			this.cmdWishlist.TabIndex = 7;
			this.cmdWishlist.Text = "Wishlist";
			this.cmdWishlist.UseVisualStyleBackColor = true;
			this.cmdWishlist.Click += new System.EventHandler(this.cmdWishlist_Click);
			// 
			// cmdRented
			// 
			this.cmdRented.Location = new System.Drawing.Point(87, 170);
			this.cmdRented.Name = "cmdRented";
			this.cmdRented.Size = new System.Drawing.Size(75, 23);
			this.cmdRented.TabIndex = 8;
			this.cmdRented.Text = "Rented";
			this.cmdRented.UseVisualStyleBackColor = true;
			this.cmdRented.Click += new System.EventHandler(this.cmdRented_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(981, 237);
			this.Controls.Add(this.cmdRented);
			this.Controls.Add(this.cmdWishlist);
			this.Controls.Add(this.cmdReturn);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.lblMovies);
			this.Controls.Add(this.cmdAddToWishlist);
			this.Controls.Add(this.cmdRent);
			this.Name = "MainForm";
			this.Text = "Movies Client Sample";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Button cmdRent;
		private System.Windows.Forms.Button cmdAddToWishlist;
		private System.Windows.Forms.Label lblMovies;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button cmdReturn;
		private System.Windows.Forms.Button cmdWishlist;
		private System.Windows.Forms.Button cmdRented;
    }
}

