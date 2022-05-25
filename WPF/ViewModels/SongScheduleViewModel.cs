using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    public class SongScheduleViewModel
    {
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public int SongId { get; set; }
        public string Title { get; set; }
    }
}
