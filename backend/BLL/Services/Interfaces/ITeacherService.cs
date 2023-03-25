using backend.BLL.Common.DTOs.Work;
using backend.BLL.Common.VMs.Group;
using backend.BLL.Common.VMs.Register;
using backend.BLL.Common.VMs.Schedule;
using backend.BLL.Common.VMs.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.BLL.Common.VMs.Attendance;

namespace backend.BLL.Services.Interfaces
{
    public interface ITeacherService
    {
        Task AddMarkAsync(string studentId, int criteriaId, int mark, string teacherId);
        Task AddWorkAsync(CreateWorkDTO model);
        Task<List<PresetViewModel>> GetPresetsAsync(int groupId, int subjectId);
        Task<List<List<RegisterRowViewModel>>> GetRegisterDataAsync(int groupId, int subjectId, bool isExtended, string teacherId);
        Task<List<ScheduleItemViewModel>> GetScheduleAsync(int dayId, string teacherId);
        Task<List<GroupInfoVM>> GetTeacherGroupsAsync(string teacherId);
        Task<List<SubjectInfoVM>> GetTeacherSubjectsInGroupAsync(string teacherId, int groupId);
        Task<List<StudentAttendanceVM>> GetStudentsAttendanceAsync(int groupId, int subjectId, DateTime date);
        Task SetStudentAttendanceAsync(string studentId, int subjectId, DateTime date, bool attendance);
    }
}
