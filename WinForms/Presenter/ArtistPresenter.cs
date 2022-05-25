using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinForms.Model;
using WinForms.View;

namespace WinForms.Presenter
{
    public class ArtistPresenter
    {
        private readonly IArtistView _view;
        private readonly IArtistRepository _repository;

        public ArtistPresenter(IArtistView view, IArtistRepository repository)
        {
            _view = view;
            view.Presenter = this;
            _repository = repository;

            UpdateArtistListView();
        }

        private void UpdateArtistListView()
        {
            var artistNames = _repository.List();
            _view.ArtistList = artistNames;
            //_view.SelectedArtist = selectedArtist;
        }

        public void UpdateArtistView(int id)
        {
            // customer list can be cached instead of re-fetching the customer each time
            // this may be infeasible if the list is large

            ArtistModel artist = _repository.GetArtist(id);
            if (artist == null) { return; }
            _view.ArtistName = artist.Name;
            _view.Description = artist.Description;
            _view.ArtistId = artist.ArtistId.ToString();
            _view.Songs = artist.Songs;
        }

        public void SaveArtist()
        {
            ArtistModel artist = new ArtistModel { ArtistId = _view.SelectedArtist, Name = _view.ArtistName, Description = _view.Description, Songs = _view.Songs };
            _repository.Save(artist);
            UpdateArtistListView();
        }
    }
}
