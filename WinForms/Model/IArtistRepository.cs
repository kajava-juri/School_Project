using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms.Model
{
    public interface IArtistRepository
    {
        IList<ArtistModel> List();
        ArtistModel GetArtist(int id);
        void Save(ArtistModel model);
        void Delete(ArtistModel model);
        IList<string> ArtistNames();
    }
}
