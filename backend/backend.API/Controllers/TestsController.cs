using backend.BLL.Common.DTOs.Tests;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestsController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet("get-tests/{subjectId}/{groupId}")]
        public async Task<ActionResult> GetTestsAsync(int subjectId, int groupId)
        {
            return Ok(await _testService.GetTestsAsync(subjectId, groupId));
        }

        [HttpGet("get-test/{testId}")]
        public async Task<ActionResult> GetTestAsync(int testId)
        {
            return Ok(await _testService.GetTestAsync(testId));
        }

        [HttpPost("create-test")]
        public async Task<ActionResult> CreateTestAsync([FromBody]TestDto entity)
        {
            await _testService.CreateTestAsync(entity);

            return Ok();
        }

        [HttpDelete("delete-test/{id}")]
        public async Task<ActionResult> DeleteTestAsync(int id)
        {
            await _testService.DeleteTestAsync(id);

            return Ok();
        }

        [HttpPost("send-test-to-review")]
        public async Task<ActionResult> SendTestToReviewAsync([FromBody]SendTestToReviewDto entity)
        {
            entity.StudentId = User.Identity.Name;
            var reuslt = await _testService.SendTestToReviewAsync(entity);

            return Ok(reuslt);
        }
    }
}
