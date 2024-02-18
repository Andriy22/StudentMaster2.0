using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DAL.Entities
{
    public class EducationMaterialGroup
    {
        [ForeignKey("EducationMaterial")] public int EducationMaterialId { get; set; }
        
        public EducationMaterial EducationMaterial { get; set; }

        [ForeignKey("Group")] public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
