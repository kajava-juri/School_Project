using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Model;
using WinForms.Presenter;

namespace WinForms.View
{
    public partial class ArtistForm : Form, IArtistView
    {
        public ArtistForm()
        {
            InitializeComponent();
        }

        public IList<ArtistModel> ArtistList 
        {
            get { return (IList<ArtistModel>)this.artistListBox.DataSource;}
            set { this.artistListBox.DataSource = value;}
        }
        public int SelectedArtist 
        {
            get { return (int)this.artistListBox.SelectedValue; }
            set { this.artistListBox.SelectedValue = value; }
        }
        public string ArtistName 
        {
            get { return this.nameTextBox.Text; }
            set { this.nameTextBox.Text = value; }
        }
        public string Description
        {
            get { return this.descriptionTextBox.Text; }
            set { this.descriptionTextBox.Text = value; }
        }
        public Presenter.ArtistPresenter Presenter 
        { 
            private get; 
            set; 
        }
        public string ArtistId 
        {
            get { return this.idTextBox.Text; }
            set { this.idTextBox.Text = value; }
        }

        public IList<SongModel> Songs 
        {
            get { return (IList<SongModel>)this.songsListView.DataSource; }
            set { this.songsListView.DataSource = value; }
        }

        private void customerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FIXME: try/catch
            var t = artistListBox.SelectedValue;
            Presenter.UpdateArtistView((int)artistListBox.SelectedValue);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.nameTextBox.ReadOnly = true;
            this.descriptionTextBox.ReadOnly = true;

            Presenter.SaveArtist();

            this.saveButton.Enabled = false;
            this.editButton.Enabled = true;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            this.editButton.Enabled = false;
            this.saveButton.Enabled = true;
            this.nameTextBox.ReadOnly = false;
            this.descriptionTextBox.ReadOnly = false;
        }
    }
}
