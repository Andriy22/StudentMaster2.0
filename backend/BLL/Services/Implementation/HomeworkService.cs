﻿using backend.BLL.Common.Consts;
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
                Attachment = new Attachment
                {
                    Path = dto.File != null ? await _fileService.SaveFile(dto.File, FileConstants.HomeworksFolder) : null
                }
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
                    NumberNotReviewed = x.HomeworkStudents.Count(y => y.Status == DAL.Enums.HomeworkStatus.NotStarted),
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
                        Deadline = x.Deadline,
                        GroupId = x.GroupId,
                        SubjectId = x.SubjectId,
                        CreatedAt = x.CreatedAt,
                        MaxGrade = x.MaxGrade,
                        SubjectName = x.Subject.Name,
                        GroupName = x.Group.Name,
                        NumberNotReviewed = x.HomeworkStudents.Count(y => y.Status == DAL.Enums.HomeworkStatus.NotStarted),
                        FilePath = x.Attachment != null ? x.Attachment.Path : null
                    }).ToListAsync();

            return homeworks;
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
