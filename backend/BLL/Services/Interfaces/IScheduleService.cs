using backend.BLL.Common.DTOs.Schedule;
using backend.BLL.Common.VMs.Schedule;

namespace backend.BLL.Services.Interfaces;

public interface IScheduleService
{
    Task<List<ScheduleDayViewModel>> GetScheduleDaysAsync();
    Task<List<ScheduleItemTypeViewModel>> GetScheduleItemTypesAsync();

    Task<List<ScheduleItemViewModel>> GetScheduleItemsAsync(int groupId, int dayId);

    Task CreateNewItemAsync(int groupId, int dayId);
    Task RemoveItemAsync(int itemId);

    Task UpdateItemAsync(EditScheduleItemDTO model);
}