using backend.BLL.Common.Exceptions;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IGroupService _groupService;

    public StudentController(IStudentService studentService, IGroupService groupService)
    {
        _studentService = studentService;
        _groupService = groupService;
    }

    [HttpGet("get-subjects")]
    [Authorize(Roles="Student")]
    public async Task<IActionResult> GetSubjects()
    {
        var group = await _groupService.GetGroupByStudentId(User.Identity.Name);

        if (group is null) throw new CustomHttpException("Invalid group");

        return Ok(await _groupService.GetGroupSubjects(group.Id));
    }

    [HttpGet("get-schedule/{dayId}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetSchedule(int dayId)
    {
        return Ok(await _studentService.GetScheduleAsync(dayId, User.Identity.Name));
    }

    [HttpGet("get-register-data/{subjectId}/{isExtended}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetRegisterData(int subjectId, bool isExtended)
    {
        return Ok(await _studentService.GetRegisterDataAsync(User.Identity.Name, subjectId, isExtended));
    }
}