using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Attendance;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.BLL.Services.Implementation;

public class AttendanceService : IAttendanceService
{
    private readonly IRepository<Attendance> _attendanceRepository;
    private readonly IGroupService _groupService;
    private readonly IRepository<User> _userRepository;

    public AttendanceService(IGroupService groupService, IRepository<Attendance> attendanceRepository,
        IRepository<User> userRepository)
    {
        _groupService = groupService;
        _attendanceRepository = attendanceRepository;
        _userRepository = userRepository;
    }

    public async Task<List<DateTime>> GetAttendanceDaysAsync(int groupId, int subjectId)
    {
        var students = await _groupService.GetStudentsByGroupId(groupId);

        var attendances = await _attendanceRepository.GetQueryable(x =>
                students.Select(s => s.Id).Contains(x.StudentId) && x.SubjectId == subjectId && x.IsPresent)
            .Select(x => x.Date)
            .ToListAsync();

        return attendances.GroupBy(x => x.ToShortDateString()).Select(x => x.First()).ToList();
    }

    public async Task<List<StudentAttendanceDateVM>> GetStudentAttendanceDatesAsync(string studentId, int subjectId)
    {
        var group = await _groupService.GetGroupByStudentId(studentId);

        if (group == null) throw new CustomHttpException("Student group not found...");

        var attendances = await _attendanceRepository
            .GetQueryable(x => x.StudentId == studentId && x.SubjectId == subjectId && x.IsPresent)
            .Select(x => new StudentAttendanceDateVM
            {
                Date = x.Date.ToShortDateString(),
                IsPresent = x.IsPresent
            })
            .ToListAsync();

        var dates = await GetAttendanceDaysAsync(group.Id, subjectId);

        var missingDates = dates.Select(x => x.ToShortDateString()).ToList().Except(attendances.Select(x => x.Date));

        attendances.AddRange(missingDates.Select(date => new StudentAttendanceDateVM
        {
            Date = date,
            IsPresent = false
        }));

        return attendances.OrderByDescending(x => x.Date).ToList();
    }
}