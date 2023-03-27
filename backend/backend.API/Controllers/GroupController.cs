using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet("get-group-info/{groupId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetGroupInfoByIdAsync(int groupId)
    {
        return Ok(await _groupService.GetGroup(groupId));
    }

    [HttpGet("get-group-students/{groupId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetStudentByGroupId(int groupId)
    {
        return Ok(await _groupService.GetStudentsByGroupId(groupId));
    }

    [HttpGet("get-group-subjects/{groupId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetGroupSubjects(int groupId)
    {
        return Ok(await _groupService.GetGroupSubjects(groupId));
    }

    [HttpGet("get-group-teachers/{groupId}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> GetGroupTeachers(int groupId)
    {
        return Ok(await _groupService.GetTeachersByGroupId(groupId));
    }
}