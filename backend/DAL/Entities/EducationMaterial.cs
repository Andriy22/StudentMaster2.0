using backend.DAL.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DAL.Entities
{
    public class EducationMaterial
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Attachment Attachment { get; set; }

        [ForeignKey("Attachment")] public int? AttachmentId { get; set; }

        public EducationMaterialType Type { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("Subject")] public int SubjectId { get; set; }

        public Subject Subject { get; set; }
      
        [ForeignKey("CreatedBy")] public string CreatedById { get; set; }

        public User CreatedBy { get; set; }

        public ICollection<EducationMaterialGroup> Groups { get; set; }
    }
}
