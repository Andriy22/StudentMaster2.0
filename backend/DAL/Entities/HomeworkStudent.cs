using backend.DAL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DAL.Entities
{
    public class HomeworkStudent
    {
        public int Id { get; set; }

        [ForeignKey("Homework")]
        public int HomeworkId { get; set; }

        public virtual Homework Homework { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }

        public virtual User Student { get; set; }

        public virtual Attachment Attachment { get; set; }

        [ForeignKey("ReviewBy")]
        public string ReviewById { get; set; }

        public virtual User ReviewedBy { get; set; }

        public HomeworkStatus Status { get; set; }

        public int? Grade { get; set; }

        public string Comment { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsModified { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set;}

        public DateTime ReviewedAt { get; set; }
    }
}
