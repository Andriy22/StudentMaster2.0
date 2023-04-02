using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpGet("get-student-attendance/{studentId}/{subjectId}")]
    [Authorize]
    public async Task<IActionResult> GetStudentAttendanceAsync(string studentId, int subjectId)
    {
        return Ok(await _attendanceService.GetStudentAttendanceDatesAsync(studentId, subjectId));
    }
}