using backend.BLL.Common.DTOs.Work;
using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Group;
using backend.BLL.Common.VMs.Register;
using backend.BLL.Common.VMs.Schedule;
using backend.BLL.Common.VMs.Subject;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using backend.BLL.Common.VMs.Attendance;
using Group = backend.DAL.Entities.Group;

namespace backend.BLL.Services.Implementation
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<EvaluationCriterion> _evaluationCriterionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<TeacherGroup> _teacherGroupRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Work> _workRepository;
        private readonly IRepository<Grade> _gradeRepository;
        private readonly IRepository<ScheduleDay> _scheduleDayRepository;
        private readonly IRepository<ScheduleItem> _scheduleItemRepository;
        private readonly IRepository<Schedule> _scheduleRepository;
        private readonly IGroupService _groupService;
        private readonly IRepository<Attendance> _attendanceRepository;

        public TeacherService(IRepository<Group> groupRepository,
            IRepository<Subject> subjectRepository,
            IRepository<User> userRepository,
            IRepository<TeacherGroup> teacherGroupRepository,
            UserManager<User> userManager,
            IRepository<Work> workRepository,
            IRepository<Grade> gradeRepository,
            IRepository<EvaluationCriterion> evaluationCriterionRepository,
            IRepository<ScheduleDay> scheduleDayRepository,
            IRepository<ScheduleItem> scheduleItemRepository,
            IRepository<Schedule> scheduleRepository,
            IGroupService groupService,
            IRepository<Attendance> attendanceRepository)
        {
            _groupRepository = groupRepository;
            _subjectRepository = subjectRepository;
            _userRepository = userRepository;
            _teacherGroupRepository = teacherGroupRepository;
            _userManager = userManager;
            _workRepository = workRepository;
            _gradeRepository = gradeRepository;
            _evaluationCriterionRepository = evaluationCriterionRepository;
            _scheduleDayRepository = scheduleDayRepository;
            _scheduleItemRepository = scheduleItemRepository;
            _scheduleRepository = scheduleRepository;
            _groupService = groupService;
            _attendanceRepository = attendanceRepository;
        }

        public async Task AddMarkAsync(string studentId, int criteriaId, int mark, string teacherId)
        {
            var student = await _userRepository.GetByIdAsync(studentId);
            var criteria = await _evaluationCriterionRepository.GetByIdAsync(criteriaId);

            if (student is null)
            {
                throw new CustomHttpException($"Student with id [{studentId}] not found");
            }

            if (criteria is null)
            {
                throw new CustomHttpException($"Column with id [{criteriaId}] not found");
            }

            var work = await _workRepository.GetQueryable(x => x.EvaluationCriteria.Contains(criteria))
                .Include(x => x.EvaluationCriteria).FirstOrDefaultAsync();

            var subjects = await GetTeacherSubjectsInGroupAsync(teacherId, work.GroupId);

            if (subjects is null)
            {
                throw new CustomHttpException($"Your subjects not found");
            }

            if (subjects.FirstOrDefault(x => x.Id == work.SubjectId) is null)
            {
                throw new CustomHttpException($"You don't have permission.");
            }

            var group = _groupRepository.GetQueryable(x => x.Students.FirstOrDefault(x => x.Id == student.Id) != null)
                .Include(x => x.Students).FirstOrDefault();

            if (group is null)
            {
                throw new CustomHttpException($"Student is not assigned to any group");
            }

            if (group.Id != work.GroupId)
            {
                throw new CustomHttpException($"You don't have permission.");
            }

            var grade = await _gradeRepository
                .GetQueryable(x => x.EvaluationCriterionId == criteria.Id && x.StudentId == studentId)
                .FirstOrDefaultAsync();

            if (grade is null)
            {
                _gradeRepository.Add(new Grade
                {
                    EvaluationCriterionId = criteriaId,
                    StudentId = studentId,
                    Value = mark,
                });

                return;
            }

            grade.Value = mark;

            _gradeRepository.Edit(grade);
        }

        public async Task AddWorkAsync(CreateWorkDTO model)
        {
            var group = await _groupRepository.GetByIdAsync(model.GroupId);
            var subject = await _subjectRepository.GetByIdAsync(model.SubjectId);

            if (group is null)
            {
                throw new CustomHttpException($"Group with id [{model.GroupId}] not found");
            }

            if (subject is null)
            {
                throw new CustomHttpException($"Subject with id [{model.GroupId}] not found");
            }


            var work = new Work
            {
                GroupId = group.Id,
                SubjectId = subject.Id,
                Name = model.Name,
            };

            foreach (var item in model.Items)
            {
                work.EvaluationCriteria.Add(new EvaluationCriterion
                {
                    MaxGrade = item.MaxGrade,
                    IsRequired = !item.Removable,
                    Name = item.Name,
                });
            }

            _workRepository.Add(work);
        }

        public async Task<List<PresetViewModel>> GetPresetsAsync(int groupId, int subjectId)
        {
            var presets = await _workRepository.GetQueryable(x => x.GroupId == groupId && x.SubjectId == subjectId)
                .Include(x => x.EvaluationCriteria).ToListAsync();

            var result = new List<PresetViewModel>();

            foreach (var preset in presets)
            {
                var p = new PresetViewModel();

                p.Name = preset.Name;

                foreach (var item in preset.EvaluationCriteria)
                {
                    p.Items.Add(new PresetItemViewModel()
                    {
                        Name = item.Name,
                        MaxGrade = item.MaxGrade,
                        Removable = !item.IsRequired
                    });
                }

                result.Add(p);
            }

            return result;
        }

        public async Task<List<List<RegisterRowViewModel>>> GetRegisterDataAsync(int groupId, int subjectId,
            bool isExtended, string teacherId)
        {
            var teacher = await _userRepository.GetByIdAsync(teacherId);
            var group = await _groupRepository.GetQueryable(x => x.Id == groupId).Include(x => x.Students)
                .FirstOrDefaultAsync();
            var subject = await _subjectRepository.GetByIdAsync(subjectId);


            if (teacher == null)
            {
                throw new CustomHttpException("Invalid teacher id!");
            }

            if (group == null)
            {
                throw new CustomHttpException("Invalid group id");
            }

            if (subject is null)
            {
                throw new CustomHttpException($"Invalid group id");
            }


            var result = new List<List<RegisterRowViewModel>>();

            foreach (var item in group.Students)
            {

                var row = new List<RegisterRowViewModel>();

                var rowData = new RegisterRowViewModel("Студент", "student");

                rowData.Items.Add(new RegisterItemViewModel
                {
                    Editable = false,
                    Id = item.Id,
                    Limit = 0,
                    Name = "student",
                    Title = "ПІБ",
                    Value = $"{item.FirstName} {item.Name} {item.LastName}",
                });

                row.Add(rowData);


                rowData = new RegisterRowViewModel("Відвідування", "visiting");

                rowData.Items.Add(new RegisterItemViewModel
                {
                    Editable = false,
                    Id = "visiting",
                    Limit = 0,
                    Name = "visiting",
                    Title = "Відвідування",
                    Value = $"0/0",
                });

                row.Add(rowData);

                foreach (var work in _workRepository.GetQueryable(x => x.GroupId == groupId && x.SubjectId == subjectId)
                             .Include(x => x.EvaluationCriteria).ToList())
                {
                    rowData = new RegisterRowViewModel(work.Name, work.Id.ToString());

                    foreach (var criteria in work.EvaluationCriteria.Where(x => x.IsRequired || isExtended).ToList())
                    {
                        rowData.Items.Add(new RegisterItemViewModel
                        {
                            Editable = true,
                            Id = item.Id,
                            Limit = criteria.MaxGrade,
                            Name = criteria.Id.ToString(),
                            Title = criteria.Name,
                            Value = _gradeRepository
                                .GetQueryable(x => x.StudentId == item.Id && x.EvaluationCriterionId == criteria.Id)
                                .FirstOrDefault()?.Value.ToString() ?? 0.ToString(),
                        });
                    }

                    row.Add(rowData);
                }


                result.Add(row);
            }


            return result;

        }

        public async Task<List<ScheduleItemViewModel>> GetScheduleAsync(int dayId, string teacherId)
        {
            var day = await _scheduleDayRepository.GetByIdAsync(dayId);

            if (day == null)
            {
                throw new CustomHttpException("Day not found");
            }

            var teacherGroups = await GetTeacherGroupsAsync(teacherId);

            var schedules = _scheduleRepository.GetQueryable(x => x.DayId == dayId).Include(x => x.Group)
                .Include(x => x.ScheduleItems).ThenInclude(x => x.ScheduleItemType);


            var result = new List<ScheduleItemViewModel>();

            foreach (var schedule in schedules)
            {
                if (!teacherGroups.Exists(x => x.Id == schedule.GroupId))
                {
                    continue;
                }

                var subjects = await GetTeacherSubjectsInGroupAsync(teacherId, schedule.GroupId);

                foreach (var subject in subjects)
                {
                    foreach (var item in schedule.ScheduleItems.Where(x => x.SubjectId == subject.Id))
                    {
                        result.Add(new ScheduleItemViewModel
                        {
                            ScheduleDay = new ScheduleDayViewModel
                            {
                                Id = day.Id,
                                Name = day.Name
                            },
                            Comment = item.Comment,
                            End = item.End,
                            Id = item.Id,
                            Position = item.Position,
                            ScheduleItemType = new ScheduleItemTypeViewModel
                            {
                                Id = item.ScheduleItemType.Id,
                                Name = item.ScheduleItemType.Name
                            },
                            Start = item.Start,
                            SubjectShortInfo = new SubjectShortInfoVM
                            {
                                Id = subject.Id,
                                Name = subject.Name
                            },
                            Subject = subject.Name,
                            Url = item.OnlineMeetingUrl
                        });
                    }
                }
            }

            return result;

        }

        public async Task<List<GroupInfoVM>> GetTeacherGroupsAsync(string teacherId)
        {
            return await _teacherGroupRepository.GetQueryable(x => x.UserId == teacherId && !x.Group.IsDeleted)
                .Include(x => x.Group).Select(group => new GroupInfoVM
                {
                    Id = group.Group.Id,
                    IsDeleted = group.Group.IsDeleted,
                    Name = group.Group.Name,
                }).ToListAsync();
        }

        public async Task<List<SubjectInfoVM>> GetTeacherSubjectsInGroupAsync(string teacherId, int groupId)
        {
            var teacher = await _userRepository.GetQueryable(x => x.Id == teacherId).Include(x => x.Subjects)
                .FirstOrDefaultAsync();
            var group = await _groupRepository.GetQueryable(x => x.Id == groupId).Include(x => x.Subjects)
                .FirstOrDefaultAsync();

            if (teacher == null)
            {
                throw new CustomHttpException("Invalid teacher id!");
            }

            if (group == null)
            {
                throw new CustomHttpException("Invalid group id");
            }

            return teacher.Subjects.Where(x => group.Subjects.Any(p => p.Id == x.Id) && !x.IsDeleted).Select(x =>
                new SubjectInfoVM
                {
                    Id = x.Id,
                    IsDeleted = x.IsDeleted,
                    Name = x.Name,
                }).ToList();
        }

        public async Task<List<StudentAttendanceVM>> GetStudentsAttendanceAsync(int groupId, int subjectId,
            DateTime date)
        {
            var result = new List<StudentAttendanceVM>();

            var students = await _groupService.GetStudentsByGroupId(groupId);

            foreach (var student in students)
            {
                var attendance = await _attendanceRepository
                    .GetQueryable(
                        x => x.SubjectId == subjectId && x.StudentId == student.Id && x.Date.Date == date.Date)
                    .FirstOrDefaultAsync();


                var res = new StudentAttendanceVM()
                {
                    FullName = student.FullName,
                    Id = student.Id,
                    IsPresent = false,
                };

                if (attendance != null)
                {
                    res.IsPresent = attendance.IsPresent;
                }

                result.Add(res);
            }

            return result;
        }

        public async Task SetStudentAttendanceAsync(string studentId, int subjectId, DateTime date, bool attendance)
        {
            var student = await _userRepository.GetByIdAsync(studentId);

            if (student == null)
            {
                throw new CustomHttpException("Student not found");
            }

            var subject = await _subjectRepository.GetByIdAsync(subjectId);

            if (subject == null)
            {
                {
                    throw new CustomHttpException("Student not found");
                }
            }


            var atd = await _attendanceRepository
                .GetQueryable(
                    x => x.SubjectId == subjectId && x.StudentId == student.Id && x.Date.Date == date.Date)
                .FirstOrDefaultAsync();

            if (atd == null)
            {
                _attendanceRepository.Add(new Attendance()
                {
                    Date = date,
                    SubjectId = subjectId,
                    StudentId = studentId,
                    IsPresent = attendance
                });

                return;
            }

            atd.IsPresent = attendance;

            _attendanceRepository.Edit(atd);

        }

    }
}
