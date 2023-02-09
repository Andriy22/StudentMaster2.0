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
        Task<IEnumerable<UserShortInfoVM>> GetStudentsByGroupId(int classId);

        Task<IEnumerable<UserShortInfoVM>> GetStudentsFromGroupByStudentId(string userId);

        Task<IEnumerable<SubjectShortInfoVM>> getTeacherClassSubjcets(string teacherId, int classId);

        void CreateGroup(string className);
        void RemoveGroup(string className);
        void RollbackGroup(string className);
        void RenameGroup(string oldName, string newName);
    }
}
