using backend.DAL.Enums;

namespace backend.BLL.Common.VMs.Homework
{
    public class HomeworkStudentViewModel
    {
        public int Id { get; set; }
        public HomeworkStatus Status { get; set; }
        public int? Grade { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsModified { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime ReviewedAt { get; set; }

        public string OwnerName { get; set; }

        public string FilePath { get; set; }

        public string ReviewerName { get; set; }
    }
}
