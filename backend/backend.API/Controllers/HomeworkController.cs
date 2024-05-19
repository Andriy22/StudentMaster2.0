using backend.BLL.Common.DTOs.Homework;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeworkController : ControllerBase
    {
        private readonly IHomeworkService _homeworkService;

        public HomeworkController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }

        [HttpGet("get-homeworks/{subjectId}/{groupId}")]
        public async Task<ActionResult> GetHomeworksAsync(int subjectId, int groupId)
        {
            return Ok(await _homeworkService.GetHomeworksAsync(subjectId, groupId));
        }

        [HttpPost("create-homework")]
        public async Task<ActionResult> CreateHomeworkAsync([FromForm]CreateHomeworkDto entity)
        {
            entity.CreatedById = User.Identity.Name;
            await _homeworkService.CreateHomeworkAsync(entity);

            return Ok();
        }

        [HttpPut("edit-homework")]
        public async Task<ActionResult> EditHomeworkAsync([FromForm]UpdateHomeworkDto entity)
        {
            await _homeworkService.UpdateHomeworkAsync(entity);

            return Ok();
        }

        [HttpDelete("delete-homework/{id}")]
        public async Task<ActionResult> DeleteHomeworkAsync(int id)
        {
            await _homeworkService.DeleteHomeworkAsync(id);

            return Ok();
        }

        [HttpGet("get-homework/{homeworkId}")]
        public async Task<ActionResult> GetHomeworkAsync(int homeworkId)
        {
            return Ok(await _homeworkService.GetHomeworkAsync(homeworkId));
        }

        [HttpPost("send-homework-to-review")]
        public async Task<ActionResult> SendHomeworkToReviewAsync([FromForm]SendHomeworkToReviewDto entity)
        {
            entity.StudentId = User.Identity.Name;
            await _homeworkService.SendHomeworkToReviewAsync(entity);

            return Ok();
        }

        [HttpGet("get-homework-student/{homeworkId}")]
        public async Task<ActionResult> GetHomeworkStudentAsync(int homeworkId, string studentId)
        {
            return Ok(await _homeworkService.GetHomeworkStudentAsync(homeworkId, User.Identity.Name));
        }

        [HttpGet("get-homeworks-for-review/{homeworkId}")]
        public async Task<ActionResult> GetHomeworksForReviewAsync(int homeworkId)
        {
            return Ok(await _homeworkService.GetHomeworksForReviewAsync(homeworkId));
        }

        [HttpPost("review-homework")]
        public async Task<ActionResult> ReviewHomeworkAsync([FromBody]ReviewHomeworkDto entity)
        {
            entity.ReviewerId = User.Identity.Name;
            await _homeworkService.ReviewHomeworkAsync(entity);

            return Ok();
        }

        [HttpDelete("cancel-homework-to-review/{id}")]
        public async Task<ActionResult> CancelHomeworkToReview(int id)
        {
            await _homeworkService.CancelHomeworkToReview(id, User.Identity.Name);

            return Ok();
        }
    }
}
