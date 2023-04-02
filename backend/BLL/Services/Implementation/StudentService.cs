using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Register;
using backend.BLL.Common.VMs.Schedule;
using backend.BLL.Common.VMs.Subject;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.BLL.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IGroupService _groupService;
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IAttendanceService _attendanceService;
        private readonly IRepository<Work> _workRepository;
        private readonly IRepository<Grade> _gradeRepository;
        private readonly IRepository<Schedule> _scheduleRepository;

        public StudentService(IRepository<User> userRepository, IGroupService groupService, IRepository<Subject> subjectRepository, IRepository<Attendance> attendanceRepository, IAttendanceService attendanceService, IRepository<Work> workRepository, IRepository<Grade> gradeRepository, IRepository<Schedule> scheduleRepository)
        {
            _userRepository = userRepository;
            _groupService = groupService;
            _subjectRepository = subjectRepository;
            _attendanceRepository = attendanceRepository;
            _attendanceService = attendanceService;
            _workRepository = workRepository;
            _gradeRepository = gradeRepository;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<List<List<RegisterRowViewModel>>> GetRegisterDataAsync(string studentId, int subjectId, bool isExtended)
        {
            var subject = await _subjectRepository.GetByIdAsync(subjectId);

            var group = await _groupService.GetGroupByStudentId(studentId);

            if (group is null) throw new CustomHttpException("Invalid group");

            if (subject is null) throw new CustomHttpException("Invalid subject id");

            var result = new List<List<RegisterRowViewModel>>();

            foreach (var item in _userRepository.GetQueryable(x=>x.Id == studentId).ToList())
            {
                var row = new List<RegisterRowViewModel>();


                var rowData = new RegisterRowViewModel("Відвідування", "visiting");


                var attendence = await _attendanceRepository
                    .GetQueryable(x => x.StudentId == item.Id && x.SubjectId == subjectId && x.IsPresent).GroupBy(x => x.Date)
                    .ToListAsync();

                rowData.Items.Add(new RegisterItemViewModel
                {
                    Editable = false,
                    Id = item.Id,
                    Limit = 0,
                    Name = "visiting",
                    Title = "Відвідування",
                    Value = $"{attendence.Count}/{(await _attendanceService.GetAttendanceDaysAsync(group.Id, subjectId)).Count}"
                });

                row.Add(rowData);

                foreach (var work in _workRepository.GetQueryable(x => x.GroupId == group.Id && x.SubjectId == subjectId)
                             .Include(x => x.EvaluationCriteria).ToList())
                {
                    rowData = new RegisterRowViewModel(work.Name, work.Id.ToString());

                    foreach (var criteria in work.EvaluationCriteria.Where(x => x.IsRequired || isExtended).ToList())
                        rowData.Items.Add(new RegisterItemViewModel
                        {
                            Editable = true,
                            Id = item.Id,
                            Limit = criteria.MaxGrade,
                            Name = criteria.Id.ToString(),
                            Title = criteria.Name,
                            Value = _gradeRepository
                                .GetQueryable(x => x.StudentId == item.Id && x.EvaluationCriterionId == criteria.Id)
                                .FirstOrDefault()?.Value.ToString() ?? 0.ToString()
                        });

                    row.Add(rowData);
                }


                result.Add(row);
            }


            return result;
        }

        public async Task<List<ScheduleItemViewModel>> GetScheduleAsync(int dayId, string studentId)
        {
            var group = await _groupService.GetGroupByStudentId(studentId);

            var schedule = await _scheduleRepository.GetQueryable(x => x.DayId == dayId && x.GroupId == group.Id)
                .Include(x => x.Day)
                .Include(x => x.ScheduleItems).ThenInclude(x => x.ScheduleItemType)
                .Include(x => x.ScheduleItems).ThenInclude(x => x.Subject)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (schedule is null) return new List<ScheduleItemViewModel>();

            return schedule.ScheduleItems.Select(itemSchedule => new ScheduleItemViewModel
            {
                ScheduleItemType = new ScheduleItemTypeViewModel
                {
                    Id = itemSchedule.ScheduleItemType.Id,
                    Name = itemSchedule.ScheduleItemType.Name,
                },
                ScheduleDay = new ScheduleDayViewModel
                {
                    Id = schedule.Day.Id,
                    Name = schedule.Day.Name,
                },
                Subject = itemSchedule.Subject.Name,
                SubjectShortInfo = new SubjectShortInfoVM()
                {
                    Id = itemSchedule.Subject.Id,
                    Name = itemSchedule.Subject.Name,
                },
                Start = itemSchedule.Start,
                End = itemSchedule.End,
                Comment = itemSchedule.Comment,
                Url = itemSchedule.OnlineMeetingUrl,
                Id = itemSchedule.Id,
                Position = itemSchedule.Position
            }).ToList();
        }
    }
}
