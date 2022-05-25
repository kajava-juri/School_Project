using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms.Model
{
    public class ArtistModel
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IList<SongModel> Songs { get; set; }
    }
}
