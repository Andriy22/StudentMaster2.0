using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ISubjectService _subjectService;
        private readonly IGroupService _groupService;

        public AdminController(IAdminService adminService, ISubjectService subjectService, IGroupService groupService)
        {
            _adminService = adminService;
            _subjectService = subjectService;
            _groupService = groupService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-roles")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            return Ok(await _adminService.GetRolesAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-subjects")]
        public async Task<IActionResult> GetAllSubjectsAsync()
        {
            return Ok(await _subjectService.GetSubjectsAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("create-subject/{subjectName}")]
        public async Task<IActionResult> CreateSubjectAsync(string subjectName)
        {
            await _subjectService.CreateSubjectAsync(subjectName);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("change-subject-status/{subjectId}")]
        public async Task<IActionResult> ChangeSubjectStatusAsync(int subjectId)
        {
            await _subjectService.ChangeSubjectStatusAsync(subjectId);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-groups")]
        public async Task<IActionResult> GetAllGroupsAsync()
        {
            return Ok(await _groupService.GetGroups());
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("create-group/{groupName}")]
        public async Task<IActionResult> CreateGroupAsync(string groupName)
        {
            await _groupService.CreateGroup(groupName);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("change-group-status/{groupId}")]
        public async Task<IActionResult> ChangeGroupStatusAsync(int groupId)
        {
            await _groupService.ChangeGroupStatus(groupId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("change-group-name/{groupId}/{newGroupName}")]
        public async Task<IActionResult> ChangeGroupStatusAsync(int groupId, string newGroupName)
        {
            await _groupService.RenameGroup(groupId, newGroupName);
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("get-users-by-role/{role}")]
        public async Task<IActionResult> GetUsersByRoleAsync(string role = "Admin")
        {
            return Ok(await _adminService.GetUsersByRoleAsync(role));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-group-by-student/{studentId}")]
        public async Task<IActionResult> GetGroupByStudentId(string studentId)
        {
            return Ok(await _groupService.GetGroupByStudentId(studentId));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-teacher-subjects/{teacherId}")]
        public async Task<IActionResult> GetTeacherSubjects(string teacherId)
        {
            return Ok(await _subjectService.GetTeacherSubjectsAsync(teacherId));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("change-student-group/{studentId}/{groupId}")]
        public async Task<IActionResult> ChangeStudentGroup(string studentId, int groupId)
        {
            await _groupService.AddStudentToGroup(groupId, studentId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("remove-student-from-group/{studentId}/{groupId}")]
        public async Task<IActionResult> RemoveStudentFromGroup(string studentId, int groupId)
        {
            await _groupService.RemoveStudentFromGroup(groupId, studentId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("add-subject-to-group/{groupId}/{subjectId}")]
        public async Task<IActionResult> AddSubjectToGroup(int groupId, int subjectId)
        {
            await _groupService.AddSubjectToGroup(groupId, subjectId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("remove-subject-from-group/{groupId}/{subjectId}")]
        public async Task<IActionResult> RemoveSubjectFromGroup(int groupId, int subjectId)
        {
            await _groupService.RemoveSubjectFromGroup(groupId, subjectId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("add-teacher-to-group/{groupId}/{teacherId}")]
        public async Task<IActionResult> AddTeacherToGroup(int groupId, string teacherId)
        {
            await _groupService.AddTeacherToGroup(groupId, teacherId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("remove-teacher-from-group/{groupId}/{teacherId}")]
        public async Task<IActionResult> RemoveTeacherFromGroup(int groupId, string teacherId)
        {
            await _groupService.RemoveTeacherFromGroup(groupId, teacherId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("add-subject-to-teacher/{subjectId}/{teacherId}")]
        public async Task<IActionResult> AddSubjectToTeacher(int subjectId, string teacherId)
        {
            await _subjectService.AddSubjectToTeacher(subjectId, teacherId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("remove-subject-from-teacher/{subjectId}/{teacherId}")]
        public async Task<IActionResult> RemoveSubjectFromTeacher(int subjectId, string teacherId)
        {
            await _subjectService.RemoveSubjectFromTeacher(subjectId, teacherId);
            return Ok();
        }
    }
}
