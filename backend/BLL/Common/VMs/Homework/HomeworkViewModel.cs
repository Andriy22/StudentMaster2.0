using backend.DAL.Enums;

namespace backend.BLL.Common.VMs.Homework
{
    public class HomeworkViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public HomeworkType Type { get; set; }
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
        public string SubjectName { get; set; }
        public string GroupName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FilePath { get; set; }
        public int MaxGrade { get; set; }
        public int NumberNotReviewed { get; set; }
    }
}
