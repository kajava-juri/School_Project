using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using WPF.Models;

namespace WPF.ViewModels
{
    internal class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly ArtistsClient _artistsClient;

        public ObservableCollection<ArtistViewModel> Artists { get; private set; }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand LoadArtists { get; set; }

        public MainWindowViewModel()
        {
            Artists = new ObservableCollection<ArtistViewModel>();
            _artistsClient = new ArtistsClient();

            SaveCommand = new RelayCommand(parameter =>
            {
                _artistsClient.Save(SelectedItem);
                
            }, parameter =>
            {
                return SelectedItem != null;
            });

            DeleteCommand = new RelayCommand(parameter =>
            {
                //_artistsClient.Delete(SelectedItem);
                Artists.Remove(SelectedItem);
            }, parameter =>
            {
                return SelectedItem != null;
            });

            LoadArtists = new RelayCommand(parameter =>
            {

            }, parameter =>
            {
                return isChecked == false;
            });
        }

        private ArtistViewModel _selectedItem;
        public ArtistViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                NotifyPropertyChanged();
            }
        }


        private bool _isChecked;
        public bool isChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                NotifyPropertyChanged();
            }
        }

        public void LoadData()
        {
            var artists = _artistsClient.List();
               foreach (var artist in artists)
            {
                Artists.Add(artist);
            }

        }
    }
}
