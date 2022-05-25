using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModels
{
    public class ScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        //public IList<SongSchedule> Songs { get; set; }
        public IList<SongScheduleViewModel> Songs { get; set; }
        public ScheduleViewModel()
        {
            Songs = new List<SongScheduleViewModel>();
        }
    }
}
