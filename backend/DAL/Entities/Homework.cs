using backend.DAL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DAL.Entities
{
    public class Homework
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Attachment Attachment { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public DateTime Deadline { get; set; }

        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }

        public int MaxGrade { get; set; }

        public HomeworkType Type { get; set; }

        public virtual User CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<HomeworkStudent> HomeworkStudents { get; set; }

    }
}
