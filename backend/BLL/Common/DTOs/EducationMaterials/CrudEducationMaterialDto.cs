
using backend.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace backend.BLL.Common.DTOs.EducationMaterials
{
    public class CrudEducationMaterialDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public EducationMaterialType Type { get; set; }

        public int SubjectId { get; set; }

        public string? Url { get; set; }

        public IFormFile? File { get; set; }

        public string UserId { get; set; }

        public List<int> Groups { get; set; }
    }
}