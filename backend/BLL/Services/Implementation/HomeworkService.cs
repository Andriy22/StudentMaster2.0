using backend.BLL.Common.Consts;
using backend.BLL.Common.DTOs.Homework;
using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Homework;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace backend.BLL.Services.Implementation
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IRepository<Homework> _homeworksRepo;
        private readonly IRepository<Attachment> _attachmentsRepo;
        private readonly IRepository<HomeworkStudent> _homeworkStudentRepo;
        private readonly IFileService _fileService;

        public HomeworkService(IRepository<Homework> homeworksRepo, IRepository<Attachment> attachmentsRepo, IRepository<HomeworkStudent> homeworkStudentRepo, IFileService fileService)
        {
            _homeworksRepo = homeworksRepo;
            _attachmentsRepo = attachmentsRepo;
            _homeworkStudentRepo = homeworkStudentRepo;
            _fileService = fileService;
        }

        public async Task CancelHomeworkToReview(int id, string studentId)
        {
            var itemToCancel = await _homeworkStudentRepo.GetQueryable(x => x.Id == id && x.StudentId == studentId)
                .FirstOrDefaultAsync();

            if (itemToCancel is null)
            {
                throw new CustomHttpException("Homework not found", HttpStatusCode.NotFound);
            }

            itemToCancel.Status = DAL.Enums.HomeworkStatus.Canceled;

            _homeworkStudentRepo.Edit(itemToCancel);
        }

        public async Task CreateHomeworkAsync(CreateHomeworkDto dto)
        {
            var homework = new Homework
            {
                Title = dto.Title,
                Description = dto.Description,
                Deadline = dto.Deadline,
                Type = dto.Type,
                SubjectId = dto.SubjectId,
                GroupId = dto.GroupId,
                MaxGrade = dto.MaxGrade,
                CreatedById = dto.CreatedById,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Attachment = dto.File != null ? new Attachment
                {
                    Path = dto.File != null ? await _fileService.SaveFile(dto.File, FileConstants.HomeworksFolder) : null
                } : null,
            };

            _homeworksRepo.Add(homework);
        }

        public async Task DeleteHomeworkAsync(int homeworkId)
        {
           var homework = await _homeworksRepo.GetQueryable(x => x.Id == homeworkId).FirstOrDefaultAsync();

            if (homework is null)
            {
                throw new CustomHttpException("Homework not found", HttpStatusCode.NotFound);
            }

            homework.IsDeleted = true;

            _homeworksRepo.Edit(homework);
        }

        public Task<HomeworkViewModel> GetHomeworkAsync(int homeworkId)
        {
            var homework = _homeworksRepo.GetQueryable(x => x.Id == homeworkId)
                .Include(x => x.Attachment)
                .Include(x => x.Subject)
                .Include(x => x.Group)
                .Include(x => x.HomeworkStudents)
                .Where(x => x.IsDeleted == false)
                .Select(x => new HomeworkViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Deadline = x.Deadline,
                    GroupId = x.GroupId,
                    SubjectId = x.SubjectId,
                    CreatedAt = x.CreatedAt,
                    MaxGrade = x.MaxGrade,
                    SubjectName = x.Subject.Name,
                    GroupName = x.Group.Name,
                    NumberNotReviewed = x.HomeworkStudents.Count(y => y.Status == DAL.Enums.HomeworkStatus.InReview),
                    FilePath = x.Attachment != null? x.Attachment.Path : null
                }).FirstOrDefaultAsync();

            return homework;
        }

        public Task<List<HomeworkViewModel>> GetHomeworksAsync(int subjectId, int groupId)
        {
            var homeworks = _homeworksRepo.GetQueryable(x => x.SubjectId == subjectId && x.GroupId == groupId)
                    .Include(x => x.Attachment)
                    .Include(x => x.Subject)
                    .Include(x => x.Group)
                    .Include(x => x.HomeworkStudents)
                    .Where(x => x.IsDeleted == false)
                    .Select(x => new HomeworkViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        Deadline = x.Deadline == default? null : x.Deadline,
                        GroupId = x.GroupId,
                        SubjectId = x.SubjectId,
                        Type = x.Type,
                        UpdatedAt = x.UpdatedAt == default? null : x.UpdatedAt,
                        CreatedAt = x.CreatedAt,
                        MaxGrade = x.MaxGrade,
                        SubjectName = x.Subject.Name,
                        GroupName = x.Group.Name,
                        NumberNotReviewed = x.HomeworkStudents.Count(y => y.Status == DAL.Enums.HomeworkStatus.InReview),
                        FilePath = x.Attachment != null ? x.Attachment.Path : null
                    }).ToListAsync();

            return homeworks;
        }

        public Task<List<HomeworkStudentViewModel>> GetHomeworksForReviewAsync(int homeworkId)
        {
            var items = _homeworkStudentRepo.GetQueryable(x => x.HomeworkId == homeworkId && x.Status != DAL.Enums.HomeworkStatus.Canceled)
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .Include(x => x.ReviewedBy)
                .Where(x => x.Homework.IsDeleted == false)
                .Select(x => new HomeworkStudentViewModel
                {
                    Id = x.Id,
                    Status = x.Status,
                    Grade = x.Grade,
                    Comment = x.Comment,
                    IsDeleted = x.IsDeleted,
                    FilePath = x.Attachment.Path,
                    IsModified = x.IsModified,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    ReviewedAt = x.ReviewedAt,
                    OwnerName = x.Student.FirstName + " " + x.Student.LastName,
                    ReviewerName = x.ReviewedBy.FirstName + " " + x.ReviewedBy.LastName
                }).ToListAsync();

            return items;
        }

        public Task<List<HomeworkStudentViewModel>> GetHomeworkStudentAsync(int homeworkId, string studentId)
        {
            var items = _homeworkStudentRepo.GetQueryable(x => x.HomeworkId == homeworkId && x.StudentId == studentId)
                .Include(x => x.Homework)
                .Include(x => x.Student)
                .Include(x => x.ReviewedBy)
                .Where(x => x.Homework.IsDeleted == false)
                .Select(x => new HomeworkStudentViewModel
                {
                    Id = x.Id,
                    Status = x.Status,
                    Grade = x.Grade,
                    Comment = x.Comment,
                    IsDeleted = x.IsDeleted,
                    IsModified = x.IsModified,
                    CreatedAt = x.CreatedAt,
                    FilePath = x.Attachment.Path,
                    UpdatedAt = x.UpdatedAt,
                    ReviewedAt = x.ReviewedAt,
                    OwnerName = x.Student.FirstName + " " + x.Student.LastName,
                    ReviewerName = x.ReviewedBy.FirstName + " " + x.ReviewedBy.LastName
                }).ToListAsync();

            return items;
        }

        public async Task ReviewHomeworkAsync(ReviewHomeworkDto dto)
        {
           var homeworkToReview = await _homeworkStudentRepo.GetQueryable(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (homeworkToReview is null)
            {
                throw new CustomHttpException("Homework not found", HttpStatusCode.NotFound);
            }

            homeworkToReview.Grade = dto.Grade;
            homeworkToReview.Comment = dto.Comment;
            homeworkToReview.ReviewById = dto.ReviewerId;
            homeworkToReview.Status = dto.Status;
            homeworkToReview.ReviewedAt = DateTime.UtcNow;

            _homeworkStudentRepo.Edit(homeworkToReview);
        }

        public async Task SendHomeworkToReviewAsync(SendHomeworkToReviewDto dto)
        {
            var newHomeworkStudent = new HomeworkStudent
            {
                HomeworkId = dto.HomeworkId,
                StudentId = dto.StudentId,
                Status = DAL.Enums.HomeworkStatus.InReview,
                Attachment = new Attachment
                {
                    Path = dto.File != null ? await _fileService.SaveFile(dto.File, FileConstants.HomeworksFolder) : null
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _homeworkStudentRepo.Add(newHomeworkStudent);
        }

        public async Task UpdateHomeworkAsync(UpdateHomeworkDto dto)
        {
            var homework = await _homeworksRepo.GetQueryable(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (homework is null)
            {
                throw new CustomHttpException("Homework not found", HttpStatusCode.NotFound);
            }

            homework.Title = dto.Title;
            homework.Description = dto.Description;
            homework.Deadline = dto.Deadline;
            homework.MaxGrade = dto.MaxGrade;
            homework.UpdatedAt = DateTime.UtcNow;

            if (dto.File is not null)
            {
                if (homework.Attachment is not null)
                {
                    _attachmentsRepo.Delete(homework.Attachment);
                }

                homework.Attachment = new Attachment
                {
                    Path = await _fileService.SaveFile(dto.File, FileConstants.HomeworksFolder)
                };
            }

            _homeworksRepo.Edit(homework);
        }
    }
}
