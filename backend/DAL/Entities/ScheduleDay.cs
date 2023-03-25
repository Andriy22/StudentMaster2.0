using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL.Entities
{
    public class ScheduleDay
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
