using Microsoft.AspNetCore.Http;

namespace backend.BLL.Common.DTOs.Homework
{
    public class SendHomeworkToReviewDto
    {
        public int HomeworkId { get; set; }
        public string StudentId { get; set; }
        public IFormFile File { get; set; }
    }
}
