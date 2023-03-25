using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.VMs.Attendance
{
    public class StudentAttendanceVM
    {
        public string Id  { get; set; }
        public string FullName { get; set; }
        public bool IsPresent { get; set; }
    }
}
