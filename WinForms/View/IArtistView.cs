using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinForms.Model;

namespace WinForms.View
{
    public interface IArtistView
    {
        IList<ArtistModel> ArtistList { get; set; }
        IList<SongModel> Songs { get; set; }
        int SelectedArtist { get; set; }

        string ArtistId { get; set; }
        string ArtistName { get; set; }
        string Description { get; set; }
        Presenter.ArtistPresenter Presenter { set; }
    }
}
