using backend.BLL.Common.Consts;
using backend.BLL.Common.DTOs.EducationMaterials;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Enums;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.BLL.Services.Implementation
{
    public class EducationMaterialService : IEducationMaterialService
    {
        private readonly IRepository<EducationMaterialGroup> educationMaterialGroupRepos;
        private readonly IRepository<EducationMaterial> educationMaterialRepo;
        private readonly UserManager<User> userManager;

        private readonly IFileService fileService;

        public EducationMaterialService(IRepository<EducationMaterialGroup> educationMaterialGroupRepos,
                                        IRepository<EducationMaterial> educationMaterialRepo,
                                        UserManager<User> userManager,
                                        IFileService fileService)
        {
            this.educationMaterialGroupRepos = educationMaterialGroupRepos;
            this.educationMaterialRepo = educationMaterialRepo;
            this.userManager = userManager;

            this.fileService = fileService;
        }

        public async Task CreateEducationMaterialAsync(CrudEducationMaterialDto entity)
        {
            var model = new EducationMaterial
            {
                Attachment = new Attachment
                {
                    Path = entity.Type == EducationMaterialType.Url ? entity.Url : await fileService.SaveFile(entity.File, FileConstants.EducationMaterialsFolder),
                },

                CreatedById = entity.UserId,
                SubjectId = entity.SubjectId,
                Title = entity.Title,
                Type = entity.Type,
                CreatedAt = DateTime.UtcNow,
                Groups = entity.Groups.Select(x => new EducationMaterialGroup
                {
                    GroupId = x
                }).ToList()
            };

            educationMaterialRepo.Add(model);
        }

        public async Task DeleteEducationMaterialAsync(int id)
        {
            var model = await educationMaterialRepo.GetQueryable(x => x.Id == id).Include(x => x.Groups).FirstOrDefaultAsync();

            if (model != null)
            {
                educationMaterialRepo.Delete(model);
            }
        }

        public async Task EditEducationMaterialAsync(CrudEducationMaterialDto entity)
        {
            var model = await educationMaterialRepo.GetQueryable(x => x.Id == entity.Id).Include(x => x.Groups).FirstOrDefaultAsync();

            if (entity.File != null && entity.Type == EducationMaterialType.File)
            {
                model.Attachment = new Attachment
                {
                    Path = await fileService.SaveFile(entity.File, FileConstants.EducationMaterialsFolder),
                };
            }

            if (entity.Type == EducationMaterialType.Url)
            {
                model.Attachment = new Attachment
                {
                    Path = entity.Url,
                };
            }

            model.CreatedById = entity.UserId;
            model.SubjectId = entity.SubjectId;
            model.Title = entity.Title;
            model.Type = entity.Type;
            model.Groups = entity.Groups.Select(x => new EducationMaterialGroup
            {
                GroupId = x
            }).ToList();

            educationMaterialRepo.Edit(model);
        }

        public async Task<List<CrudEducationMaterialDto>> GetEducationMaterialsAsync(int subjectId, int? groupId = null)
        {
            var entities = await educationMaterialRepo.GetQueryable(x => x.SubjectId == subjectId && (!groupId.HasValue || groupId == 0 || x.Groups.Any(g => g.GroupId == groupId)))
                                                      .Include(x => x.Attachment)
                                                      .Include(x => x.Groups)
                                                      .ToListAsync();

            return entities.Select(x => new CrudEducationMaterialDto
            {
                Url = x.Attachment.Path,
                Groups = x.Groups.Select(g => g.GroupId).ToList(),
                Type = x.Type,
                Id = x.Id,
                SubjectId = subjectId,
                Title = x.Title,
                UserId = x.CreatedById
            }).ToList();
        }
    }
}
