using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.DTOs.Work
{
    public class CreateWorkItemDTO
    {
        public string Name { get; set; }
        public int MaxGrade { get; set; }
        public bool Removable { get; set; }
    }
}
