using backend.BLL.Common.VMs.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.VMs.Schedule
{
    public class ScheduleItemViewModel
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string Subject { get; set; }
        public string Url { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Comment { get; set; }
        public SubjectShortInfoVM SubjectShortInfo { get; set; }
        public ScheduleDayViewModel ScheduleDay { get; set; }
        public ScheduleItemTypeViewModel ScheduleItemType { get; set; }
    }
}
