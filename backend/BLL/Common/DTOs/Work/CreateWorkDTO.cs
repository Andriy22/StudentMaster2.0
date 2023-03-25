using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.DTOs.Work
{
    public class CreateWorkDTO
    {
        public string Name { get; set; }
        public int GroupId { get; set; }
        public int SubjectId { get; set; }
        public List<CreateWorkItemDTO> Items { get; set; }
    }
}
