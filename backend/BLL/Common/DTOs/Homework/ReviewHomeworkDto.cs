using backend.DAL.Enums;

namespace backend.BLL.Common.DTOs.Homework
{
    public class ReviewHomeworkDto
    {
        public int Id { get; set; }
        public int? Grade { get; set; }
        public string Comment { get; set; }
        public string ReviewerId { get; set; }
        public HomeworkStatus Status { get; set; }
    }
}
