using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.DTOs.Tests
{
    public class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
        public DateTime Deadline { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}
