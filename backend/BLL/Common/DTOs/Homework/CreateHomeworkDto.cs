using backend.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace backend.BLL.Common.DTOs.Homework
{
    public class CreateHomeworkDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public HomeworkType Type { get; set; }
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
        public int MaxGrade { get; set; }
        public IFormFile File { get; set; }
        public string CreatedById { get; set; }
    }
}
