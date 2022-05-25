using System.Collections.Generic;
using System.Net;
using WPF.ViewModels;
using Newtonsoft.Json;
using System;

namespace WPF
{
    public class SchedulesClient
    {
        public IList<ScheduleViewModel> List()
        {
            using (var client = new WebClient())
            {
                var data = client.DownloadString("http://localhost:63150/api/schedule");

                return JsonConvert.DeserializeObject<List<ScheduleViewModel>>(data);
            }
        }

        public void Save(ScheduleViewModel model)
        {
            var artist = JsonConvert.SerializeObject(model);
            using (var client = new WebClient())
            {
                client.Headers.Set("Content-Type", "application/json");
                client.UploadString("http://localhost:63150/api/artist/" + model.ScheduleId, "PUT", artist);

            }
        }

        public void Delete(ScheduleViewModel model)
        {
            using (var client = new WebClient())
            {
                client.UploadData("http://localhost:63150/api/artist/" + model.ScheduleId, "DELETE", new byte[1]);
            }
        }
    }
}
