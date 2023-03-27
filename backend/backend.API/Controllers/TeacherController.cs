using backend.BLL.Common.DTOs.Work;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet("get-my-groups")]
    public async Task<IActionResult> GetMyGroupsAsync()
    {
        return Ok(await _teacherService.GetTeacherGroupsAsync(User.Identity.Name));
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet("get-my-subjects-in-group/{groupId}")]
    public async Task<IActionResult> GeyMySubjectsInGroupAsync(int groupId)
    {
        return Ok(await _teacherService.GetTeacherSubjectsInGroupAsync(User.Identity.Name, groupId));
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet("get-presets/{groupId}/{subjectId}")]
    public async Task<IActionResult> GetPresetsAsync(int groupId, int subjectId)
    {
        return Ok(await _teacherService.GetPresetsAsync(groupId, subjectId));
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost("add-work")]
    public async Task<IActionResult> AddWorkAsync(CreateWorkDTO model)
    {
        await _teacherService.AddWorkAsync(model);

        return Ok();
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet("set-student-attendance/{studentId}/{subjectId}/{date}/{attendance}")]
    public async Task<IActionResult> SetStudentAttendanceAsync(string studentId, int subjectId, DateTime date,
        bool attendance)
    {
        await _teacherService.SetStudentAttendanceAsync(studentId, subjectId, date, attendance);
        return Ok();
    }


    [Authorize(Roles = "Teacher")]
    [HttpGet("get-students-attendance/{groupId}/{subjectId}/{date}")]
    public async Task<IActionResult> GetStudentsAttendanceAsync(int groupId, int subjectId, DateTime date)
    {
        return Ok(await _teacherService.GetStudentsAttendanceAsync(groupId, subjectId, date));
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet("add-grade/{studentId}/{criteriaId}/{mark}")]
    public async Task<IActionResult> AddGradeAsync(string studentId, int criteriaId, int mark)
    {
        await _teacherService.AddMarkAsync(studentId, criteriaId, mark, User.Identity.Name);

        return Ok();
    }


    [Authorize(Roles = "Teacher")]
    [HttpGet("get-schedule/{dayId}")]
    public async Task<IActionResult> GetSchedule(int dayId)
    {
        return Ok(await _teacherService.GetScheduleAsync(dayId, User.Identity.Name));
    }

    [Authorize(Roles = "Teacher, Admin")]
    [HttpGet("get-register-data/{groupId}/{subjectId}/{isExtended}")]
    public async Task<IActionResult> GetRegisterDataAsync(int groupId, int subjectId, bool isExtended)
    {
        return Ok(await _teacherService.GetRegisterDataAsync(groupId, subjectId, isExtended, User.Identity.Name));
    }
}