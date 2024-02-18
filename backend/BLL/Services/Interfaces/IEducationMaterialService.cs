using backend.BLL.Common.DTOs.EducationMaterials;

namespace backend.BLL.Services.Interfaces
{
    public interface IEducationMaterialService
    {
        public Task CreateEducationMaterialAsync(CrudEducationMaterialDto entity);
        public Task DeleteEducationMaterialAsync(int  id);
        public Task EditEducationMaterialAsync(CrudEducationMaterialDto entity);

        public Task<List<CrudEducationMaterialDto>> GetEducationMaterialsAsync(int subjectId, int? groupId = null);
    }
}