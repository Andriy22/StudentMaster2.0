using System.Net;
using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Group;
using backend.BLL.Common.VMs.Subject;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.BLL.Services.Implementation;

public class GroupService : IGroupService
{
    private readonly IRepository<Group> _groupRepository;
    private readonly IRepository<Subject> _subjectRepository;
    private readonly IRepository<TeacherGroup> _teacherGroupRepository;
    private readonly UserManager<User> _userManager;
    private readonly IRepository<User> _userRepository;

    public GroupService(IRepository<Group> groupRepository, IRepository<Subject> subjectRepository,
        IRepository<User> userRepository, IRepository<TeacherGroup> teacherGroupRepository,
        UserManager<User> userManager)
    {
        _groupRepository = groupRepository;
        _subjectRepository = subjectRepository;
        _userRepository = userRepository;
        _teacherGroupRepository = teacherGroupRepository;
        _userManager = userManager;
    }

    public async Task AddStudentToGroup(int groupId, string studentId)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);

        if (group == null) throw new CustomHttpException($"Group with id [{groupId}] not found!");

        var student = await _userRepository.GetByIdAsync(studentId);

        if (student == null) throw new CustomHttpException($"Student with id [{studentId}] not found!");

        student.Group = group;

        _userRepository.Edit(student);
    }

    public async Task AddSubjectToGroup(int groupId, int subjectId)
    {
        var group = await _groupRepository.GetQueryable(x => x.Id == groupId).Include(x => x.Subjects)
            .FirstOrDefaultAsync();
        if (group == null) throw new CustomHttpException($"Group with id [{groupId}] not found!");

        var subject = await _subjectRepository.GetByIdAsync(subjectId);

        if (subject == null) throw new CustomHttpException($"Subject with id [{subjectId}] not found!");

        if (group.Subjects.Contains(subject))
            throw new CustomHttpException($"Group [{group.Name}] already have subject [{subject.Name}]!");

        group.Subjects.Add(subject);

        _groupRepository.Edit(group);
    }

    public async Task AddTeacherToGroup(int groupId, string teacherId)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);

        if (group == null) throw new CustomHttpException($"Group with id [{groupId}] not found!");

        var teacher = await _userRepository.GetByIdAsync(teacherId);

        if (teacher == null) throw new CustomHttpException($"Teacher with id [{teacherId}] not found!");

        if (!await _userManager.IsInRoleAsync(teacher, "Teacher"))
            throw new CustomHttpException("User has not teacher role");

        group.TeacherGroups.Add(new TeacherGroup
        {
            GroupId = groupId,
            UserId = teacherId
        });

        _groupRepository.Edit(group);
    }

    public async Task ChangeGroupStatus(int groupId)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);
        if (group == null) throw new CustomHttpException($"Group with id [{groupId}] not found!");

        if (group.IsDeleted)
            if (await _groupRepository.GetSingleAsync(x =>
                    x.Name.ToLower() == group.Name.ToLower() && x.Id != group.Id && !x.IsDeleted) != null)
                group.Name += "_recovered";

        group.IsDeleted = !group.IsDeleted;
        _groupRepository.Edit(group);
    }

    public async Task CreateGroup(string groupName)
    {
        var group = await _groupRepository.GetQueryable(x => x.Name.ToLower() == groupName.ToLower())
            .FirstOrDefaultAsync();
        if (group != null) throw new CustomHttpException("Group name [" + groupName + "] is already used!");

        _groupRepository.Add(new Group
        {
            Name = groupName
        });
    }

    public async Task<GroupInfoVM> GetGroup(int groupId)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);
        if (group == null)
            throw new CustomHttpException($"Group with id [{groupId}] not found!", HttpStatusCode.NotFound);

        return new GroupInfoVM
        {
            Id = group.Id,
            IsDeleted = group.IsDeleted,
            Name = group.Name
        };
    }

    public async Task<GroupInfoVM> GetGroupByStudentId(string studentId)
    {
        var group = await _groupRepository.GetQueryable(x => x.Students.FirstOrDefault(x => x.Id == studentId) != null)
            .Include(x => x.Students).FirstOrDefaultAsync();

        if (group == null) throw new CustomHttpException("Group not found...");

        return new GroupInfoVM
        {
            Id = group.Id,
            IsDeleted = group.IsDeleted,
            Name = group.Name
        };
    }

    public async Task<IEnumerable<GroupInfoVM>> GetGroups()
    {
        var result = new List<GroupInfoVM>();

        foreach (var el in await _groupRepository.GetAsync())
            result.Add(new GroupInfoVM { Id = el.Id, Name = el.Name, IsDeleted = el.IsDeleted });

        return result;
    }

    public async Task<IEnumerable<SubjectShortInfoVM>> GetGroupSubjects(int groupId)
    {
        var group = await _groupRepository.GetQueryable(x => x.Id == groupId).Include(x => x.Subjects)
            .FirstOrDefaultAsync();
        if (group == null)
            throw new CustomHttpException($"Group with id [{groupId}] not found!", HttpStatusCode.NotFound);

        var result = new List<SubjectShortInfoVM>();

        foreach (var item in group.Subjects)
            result.Add(new SubjectShortInfoVM
            {
                Id = item.Id,
                Name = item.Name
            });

        return result;
    }

    public async Task<IEnumerable<UserShortInfoVM>> GetStudentsByGroupId(int groupId)
    {
        var students = new List<UserShortInfoVM>();
        var group = await _groupRepository.GetQueryable(x => x.Id == groupId).Include(x => x.Students)
            .FirstOrDefaultAsync();

        if (group == null) throw new CustomHttpException("Group not found...");


        foreach (var el in group.Students)
            students.Add(new UserShortInfoVM
            {
                Id = el.Id,
                FullName = el.FirstName + ' ' + el.Name + ' ' + el.LastName,
                IsDeleted = el.IsDeleted
            });
        return students;
    }

    public async Task<IEnumerable<UserShortInfoVM>> GetStudentsFromGroupByStudentId(string userId)
    {
        var user = await _userRepository.GetQueryable(x => x.Id == userId).Include(x => x.Group).FirstOrDefaultAsync();
        if (user == null) throw new CustomHttpException("User not found...", HttpStatusCode.NotFound);

        var userGroup = await _groupRepository.GetQueryable(x => x.Id == user.Group.Id).Include(x => x.Students)
            .FirstOrDefaultAsync();
        if (userGroup == null) throw new CustomHttpException("Group not found...", HttpStatusCode.NotFound);

        var students = new List<UserShortInfoVM>();

        foreach (var el in userGroup.Students)
            students.Add(new UserShortInfoVM
            {
                Id = el.Id,
                FullName = el.FirstName + ' ' + el.Name + ' ' + el.LastName
            });


        return students;
    }

    public async Task<IEnumerable<SubjectShortInfoVM>> GetTeacherGroupSubjcets(string teacherId, int groupId)
    {
        var teacher = await _userRepository.GetQueryable(x => x.Id == teacherId).Include(x => x.Subjects)
            .FirstOrDefaultAsync();
        var group = await _groupRepository.GetQueryable(x => x.Id == groupId).Include(x => x.Subjects)
            .FirstOrDefaultAsync();

        if (teacher == null) throw new CustomHttpException("User not found...", HttpStatusCode.NotFound);

        if (group == null) throw new CustomHttpException("Group not found...", HttpStatusCode.NotFound);

        var result = new List<SubjectShortInfoVM>();
        foreach (var el in teacher.Subjects)
            if (group.Subjects.FirstOrDefault(x => x.Id == el.Id) != null)
                result.Add(new SubjectShortInfoVM { Id = el.Id, Name = el.Name });
        return result;
    }

    public async Task<IEnumerable<UserShortInfoVM>> GetTeachersByGroupId(int groupId)
    {
        var result = new List<UserShortInfoVM>();

        var group = await _groupRepository.GetQueryable(x => x.Id == groupId).Include(x => x.TeacherGroups)
            .ThenInclude(x => x.User).FirstOrDefaultAsync();

        if (group == null) throw new CustomHttpException("Group not found...", HttpStatusCode.NotFound);

        foreach (var item in group.TeacherGroups)
            result.Add(new UserShortInfoVM
            {
                IsDeleted = item.User.IsDeleted,
                Id = item.User.Id,
                FullName = item.User.FirstName + ' ' + item.User.Name + ' ' + item.User.LastName
            });

        return result;
    }

    public async Task RemoveStudentFromGroup(int groupId, string studentId)
    {
        var group = await _groupRepository.GetQueryable(x => x.Id == groupId).Include(x => x.Students)
            .FirstOrDefaultAsync();
        if (group == null) throw new CustomHttpException($"Group with id [{groupId}] not found!");

        var student = await _userRepository.GetByIdAsync(studentId);

        if (student == null) throw new CustomHttpException($"Student with id [{studentId}] not found!");

        if (!group.Students.Contains(student)) return;

        group.Students.Remove(student);

        _groupRepository.Edit(group);
    }

    public async Task RemoveSubjectFromGroup(int groupId, int subjectId)
    {
        var group = await _groupRepository.GetQueryable(x => x.Id == groupId).Include(x => x.Subjects)
            .FirstOrDefaultAsync();
        if (group == null) throw new CustomHttpException($"Group with id [{groupId}] not found!");

        var subject = await _subjectRepository.GetByIdAsync(subjectId);

        if (subject == null) throw new CustomHttpException($"Subject with id [{subjectId}] not found!");

        if (!group.Subjects.Contains(subject)) return;

        group.Subjects.Remove(subject);

        _groupRepository.Edit(group);
    }

    public async Task RemoveTeacherFromGroup(int groupId, string teacherId)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);

        if (group == null) throw new CustomHttpException($"Group with id [{groupId}] not found!");

        var teacher = await _userRepository.GetByIdAsync(teacherId);

        if (teacher == null) throw new CustomHttpException($"Teacher with id [{teacherId}] not found!");

        if (!await _userManager.IsInRoleAsync(teacher, "Teacher"))
            throw new CustomHttpException("User has not teacher role!");

        var teacherGroup = _teacherGroupRepository.GetQueryable(x => x.GroupId == groupId && x.UserId == teacherId)
            .Include(x => x.User).Include(x => x.Group).FirstOrDefault();

        if (teacherGroup == null) throw new CustomHttpException("Teacher is not in group!");

        _teacherGroupRepository.Delete(teacherGroup);
    }

    public async Task RenameGroup(int groupId, string newName)
    {
        var cl = await _groupRepository.GetByIdAsync(groupId);
        if (cl == null) throw new CustomHttpException("Group not found...", HttpStatusCode.NotFound);

        if (string.IsNullOrEmpty(newName))
            throw new CustomHttpException("New group name can't be empty!", HttpStatusCode.NotFound);

        if (_groupRepository.GetQueryable(x => x.Name.ToLower() == newName.ToLower()).FirstOrDefault() != null)
            throw new CustomHttpException("Group name [" + newName + "] is already used!");

        cl.Name = newName;
        _groupRepository.Edit(cl);
    }
}