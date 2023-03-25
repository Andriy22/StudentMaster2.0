using backend.BLL.Common.DTOs.Schedule;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("get-schedule-group/{groupId}/day/{dayId}")]
        public async Task<IActionResult> GetScheduleItems(int groupId, int dayId)
        {
            var viewModel = await _scheduleService.GetScheduleItemsAsync(groupId, dayId);

            return Ok(viewModel);
        }

        [HttpGet("get-days")]
        public async Task<IActionResult> GetScheduleDays()
        {
            var viewModel = await _scheduleService.GetScheduleDaysAsync();

            return Ok(viewModel);
        }

        [HttpGet("get-item-types")]
        public async Task<IActionResult> GetScheduleItemTypes()
        {
            var viewModel = await _scheduleService.GetScheduleItemTypesAsync();

            return Ok(viewModel);
        }

        [HttpGet("create-new-item/{groupId}/{dayId}")]
        public async Task<IActionResult> CreateNewItem(int groupId, int dayId)
        {
            await _scheduleService.CreateNewItemAsync(groupId, dayId);
            return Ok();
        }

        [HttpDelete("remove-item/{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            await _scheduleService.RemoveItemAsync(itemId);
            return Ok();
        }

        [HttpPost("update-item")]
        public async Task<IActionResult> UpdateItem(EditScheduleItemDTO model)
        {
            await _scheduleService.UpdateItemAsync(model);
            return Ok();
        }
    }
}
