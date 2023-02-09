using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Group;
using backend.BLL.Common.VMs.Subject;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Implementation
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<User> _userRepository;


        public void CreateGroup(string className)
        {
            var group = _groupRepository.GetQueryable(x => x.Name.ToLower() == className.ToLower()).FirstOrDefault();
            if (group != null)
            {
                throw new CustomHttpException("Group name [" + className + "] is used");
            }

            _groupRepository.Add(new Group()
            {
                Name = className
            });
        }

        public async Task<IEnumerable<UserShortInfoVM>> GetStudentsByGroupId(int classId)
        {
            var students = new List<UserShortInfoVM>();
            var pos = 1;
            var group = (await _groupRepository.GetQueryable(x => x.Id == classId).Include(x => x.Students).FirstOrDefaultAsync());

            if (group == null)
            {
                throw new CustomHttpException("Group not found...", System.Net.HttpStatusCode.NotFound);
            }
              

            foreach (var el in group.Students)
            {
                students.Add(new UserShortInfoVM()
                {
                    Id = el.Id,
                    FullName = el.FirstName + ' ' + el.Name + ' ' + el.LastName
                });
                pos++;
            }
            return students;
        }

        public async Task<IEnumerable<UserShortInfoVM>> GetStudentsFromGroupByStudentId(string userId)
        {
            var user = await this._userRepository.GetQueryable(x => x.Id == userId).Include(x => x.Group).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new CustomHttpException("User not found...", System.Net.HttpStatusCode.NotFound);
            }

            var userGroup = await _groupRepository.GetQueryable(x => x.Id == user.Group.Id).Include(x => x.Students).FirstOrDefaultAsync();
            if (userGroup == null)
            {
                throw new CustomHttpException("Group not found...", System.Net.HttpStatusCode.NotFound);
            }

            var students = new List<UserShortInfoVM>();

            foreach (var el in userGroup.Students)
            {
                students.Add(new UserShortInfoVM()
                {
                    Id = el.Id,
                    FullName = el.FirstName + ' ' + el.Name + ' ' + el.LastName
                });
            }
              

            return students;
        }

        public async Task<IEnumerable<SubjectShortInfoVM>> getTeacherClassSubjcets(string teacherId, int classId)
        {
            var teacher = await _userRepository.GetQueryable(x => x.Id == teacherId).Include(x => x.Subjects).FirstOrDefaultAsync();
            var group = await _groupRepository.GetQueryable(x => x.Id == classId).Include(x => x.Subjects).FirstOrDefaultAsync();

            if (teacher == null)
            {
                throw new CustomHttpException("User not found...", System.Net.HttpStatusCode.NotFound);
            }

            if (group == null)
            {
                throw new CustomHttpException("Group not found...", System.Net.HttpStatusCode.NotFound);
            }

            var result = new List<SubjectShortInfoVM>();
            foreach (var el in teacher.Subjects)
            {
                if (group.Subjects.FirstOrDefault(x => x.Id == el.Id) != null)
                {
                    result.Add(new SubjectShortInfoVM { Id = el.Id, Name = el.Name });
                }
            }
            return result;
        }

        public void RemoveGroup(string className)
        {
            var cl = _groupRepository.GetQueryable(x => x.Name.ToLower() == className.ToLower() && x.IsDeleted == false).FirstOrDefault();
            if (cl == null)
            {
                throw new CustomHttpException("Class name [" + className + "] not found", System.Net.HttpStatusCode.NotFound);
            }
              
            cl.IsDeleted = true;
            _groupRepository.Edit(cl);
        }

        public void RenameGroup(string oldName, string newName)
        {
            var cl = _groupRepository.GetQueryable(x => x.Name.ToLower() == oldName.ToLower()).FirstOrDefault();
            if (cl == null)
            {
                throw new CustomHttpException("Group not found...", System.Net.HttpStatusCode.NotFound);
            }
            
            if (string.IsNullOrEmpty(newName))
            {
                throw new CustomHttpException("New group name can't be empty!", System.Net.HttpStatusCode.NotFound);
            }

            cl.Name = newName;
            _groupRepository.Edit(cl);
        }

        public void RollbackGroup(string className)
        {
            var cl = _groupRepository.GetQueryable(x => x.Name.ToLower() == className.ToLower() && x.IsDeleted == true).FirstOrDefault();
            if (cl == null)
            {
                throw new CustomHttpException("Class name [" + className + "] not found", System.Net.HttpStatusCode.NotFound);
            }

            cl.IsDeleted = false;
            _groupRepository.Edit(cl);
        }
    }
}
