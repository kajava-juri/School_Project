using System.Collections.Generic;
using System.Net;
using WPF.ViewModels;
using Newtonsoft.Json;
using System;

namespace WPF
{
    public class ArtistsClient
    {
        public IList<ArtistViewModel> List()
        {
            using (var client = new WebClient())
            {
                var data = client.DownloadString("http://localhost:63150/api/artist");

                return JsonConvert.DeserializeObject<List<ArtistViewModel>>(data);
            }
        }

        public void Save(ArtistViewModel model)
        {
            var artist = JsonConvert.SerializeObject(model);
            using (var client = new WebClient())
            {
                client.Headers.Set("Content-Type", "application/json");
                client.UploadString("http://localhost:63150/api/artist/" + model.ArtistId, "PUT", artist);
                
            }
        }

        public void Delete(ArtistViewModel model)
        {
            using (var client = new WebClient())
            {
                client.UploadData("http://localhost:63150/api/artist/" + model.ArtistId, "DELETE", new byte[1]);
            }
        } 
    }
}
