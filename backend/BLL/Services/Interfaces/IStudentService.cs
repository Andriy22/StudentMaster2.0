using backend.BLL.Common.VMs.Register;
using backend.BLL.Common.VMs.Schedule;

namespace backend.BLL.Services.Interfaces;

public interface IStudentService
{
    Task<List<List<RegisterRowViewModel>>> GetRegisterDataAsync(string studentId, int subjectId, bool isExtended);
    Task<List<ScheduleItemViewModel>> GetScheduleAsync(int dayId, string studentId);
}