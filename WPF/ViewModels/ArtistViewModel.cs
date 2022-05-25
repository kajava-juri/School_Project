using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Models;

namespace WPF.ViewModels
{
    public class ArtistViewModel : NotifyPropertyChangedBase
    {
        public int ArtistId { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                NotifyPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;

                NotifyPropertyChanged();
            }
        }

        public IList<SongModel> Songs { get; set; }
    }
}
