using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WinForms.Model
{
    public class ArtistClient : IArtistRepository
    {
        public IList<ArtistModel> List()
        {
            using (var client = new WebClient())
            {
                var data = client.DownloadString("http://localhost:63150/api/artist");

                return JsonConvert.DeserializeObject<List<ArtistModel>>(data);
            }
        }
        public IList<string> ArtistNames()
        {
            using (var client = new WebClient())
            {
                var data = client.DownloadString("http://localhost:63150/api/artist");

                return JsonConvert.DeserializeObject<List<ArtistModel>>(data).Select(a => a.Name).ToList();
            }
        }
        public ArtistModel GetArtist(int id)
        {
            using(var client = new WebClient())
            {
                var data = client.DownloadString("http://localhost:63150/api/artist/" + id);

                return JsonConvert.DeserializeObject<ArtistModel>(data);
            }
        }

        public void Save(ArtistModel model)
        {
            var artist = JsonConvert.SerializeObject(model);
            using (var client = new WebClient())
            {
                client.Headers.Set("Content-Type", "application/json");
                client.UploadString("http://localhost:63150/api/artist/" + model.ArtistId, "PUT", artist);

            }
        }

        public void Delete(ArtistModel model)
        {
            using (var client = new WebClient())
            {
                client.UploadData("http://localhost:63150/api/artist/" + model.ArtistId, "DELETE", new byte[1]);
            }
        }
    }
}
