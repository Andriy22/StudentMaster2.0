using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Subject;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Implementation
{
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<Subject> _subjectRepo;
        private readonly UserManager<User> _userManager;

        public SubjectService(IRepository<Subject> subjectRepo, UserManager<User> userManager)
        {
            _subjectRepo = subjectRepo;
            _userManager = userManager;
        }

        public async Task AddSubjectToTeacher(int subjectId, string teacherId)
        {
            var user = await _userManager.FindByIdAsync(teacherId);

            if (user == null)
            {
                throw new CustomHttpException($"Teacher with id [{teacherId}] not found!");
            }

            if (!await _userManager.IsInRoleAsync(user, "Teacher"))
            {
                throw new CustomHttpException($"User is not the teacher");
            }

            var subject = await _subjectRepo.GetQueryable(x=>x.Id == subjectId).Include(x=>x.Teachers).FirstOrDefaultAsync();
            if (subject == null)
            {
                throw new CustomHttpException($"Subject with id [{subjectId}] not found!");
            }

            if (subject.Teachers.Contains(user))
            {
                return;
            }

            subject.Teachers.Add(user);

            _subjectRepo.Edit(subject);
        }

        public async Task ChangeSubjectStatusAsync(int subjectId)
        {
            var subject = await _subjectRepo.GetByIdAsync(subjectId);
            if (subject == null)
            {
                throw new CustomHttpException($"Subject with id [{subjectId}] not found!");
            }

            subject.IsDeleted = !subject.IsDeleted;
            _subjectRepo.Edit(subject);
        }

        public async Task CreateSubjectAsync(string subjectName)
        {
            if (string.IsNullOrEmpty(subjectName))
            {
                throw new CustomHttpException($"Subject name can't be empty!");
            }

            var subject = await _subjectRepo.GetQueryable(x=>x.Name.ToLower() == subjectName.ToLower()).FirstOrDefaultAsync();
            if (subject != null)
            {
                throw new CustomHttpException($"Subject with name [{subjectName}] already exists!");
            }

           _subjectRepo.Add(new Subject
           {
               IsDeleted = false,
               Name = subjectName
           });
        }

        public async Task<List<SubjectInfoVM>> GetSubjectsAsync()
        {
            var result = new List<SubjectInfoVM>();

            foreach (var el in await _subjectRepo.GetAsync())
            {
                result.Add(new SubjectInfoVM() { Id = el.Id, Name = el.Name, IsDeleted = el.IsDeleted });
            }

            return result;
        }

        public async Task<List<SubjectInfoVM>> GetTeacherSubjectsAsync(string teacherId)
        {
            var subjects = await _subjectRepo.GetQueryable(x => x.Teachers.FirstOrDefault(x => x.Id == teacherId) != null).Include(x => x.Teachers).ToListAsync();

            var result = new List<SubjectInfoVM>();

            foreach (var el in subjects)
            {
                result.Add(new SubjectInfoVM() { Id = el.Id, Name = el.Name, IsDeleted = el.IsDeleted });
            }

            return result;
        }

        public async Task RemoveSubjectFromTeacher(int subjectId, string teacherId)
        {
            var user = await _userManager.FindByIdAsync(teacherId);

            if (user == null)
            {
                throw new CustomHttpException($"Teacher with id [{teacherId}] not found!");
            }

            if (!await _userManager.IsInRoleAsync(user, "Teacher"))
            {
                throw new CustomHttpException($"User is not the teacher");
            }

            var subject = await _subjectRepo.GetQueryable(x => x.Id == subjectId).Include(x => x.Teachers).FirstOrDefaultAsync();
            if (subject == null)
            {
                throw new CustomHttpException($"Subject with id [{subjectId}] not found!");
            }

            if (!subject.Teachers.Contains(user))
            {
                return;
            }

            subject.Teachers.Remove(user);

            _subjectRepo.Edit(subject);
        }
    }
}
