using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public int DayId { get; set; }
        public ScheduleDay Day { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<ScheduleItem> ScheduleItems { get; set; }
    }
}
