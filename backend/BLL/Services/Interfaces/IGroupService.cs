using backend.BLL.Common.VMs.Group;
using backend.BLL.Common.VMs.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<UserShortInfoVM>> GetStudentsByGroupId(int groupId);

        Task<IEnumerable<UserShortInfoVM>> GetTeachersByGroupId(int groupId);

        Task<IEnumerable<UserShortInfoVM>> GetStudentsFromGroupByStudentId(string userId);

        Task<IEnumerable<SubjectShortInfoVM>> GetTeacherGroupSubjcets(string teacherId, int groupId);
        Task<IEnumerable<SubjectShortInfoVM>> GetGroupSubjects(int groupId);
        Task<IEnumerable<GroupInfoVM>> GetGroups();
        Task<GroupInfoVM> GetGroup(int groupId);
        Task<GroupInfoVM> GetGroupByStudentId(string studentId);
        Task CreateGroup(string groupName);
        Task ChangeGroupStatus(int groupId);
        Task RenameGroup(int groupId, string newName);
        Task AddStudentToGroup(int groupId, string studentId);
        Task RemoveStudentFromGroup(int groupId, string studentId);

        Task AddSubjectToGroup(int groupId, int subjectId);
        Task RemoveSubjectFromGroup(int groupId, int subjectId);
        Task AddTeacherToGroup(int groupId, string teacherId);
        Task RemoveTeacherFromGroup(int groupId, string teacherId);

    }
}
