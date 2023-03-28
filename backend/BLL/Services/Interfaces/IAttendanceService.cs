using backend.BLL.Common.VMs.Attendance;

namespace backend.BLL.Services.Interfaces;

public interface IAttendanceService
{
    Task<List<DateTime>> GetAttendanceDaysAsync(int groupId, int subjectId);
    Task<List<StudentAttendanceDateVM>> GetStudentAttendanceDatesAsync(string studentId, int subjectId);
}