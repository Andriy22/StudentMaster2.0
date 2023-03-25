using backend.BLL.Common.DTOs.Schedule;
using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Schedule;
using backend.BLL.Common.VMs.Subject;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace backend.BLL.Services.Implementation
{
    public class ScheduleService : IScheduleService
    {
        private readonly IRepository<ScheduleItem> _scheduleItemRepository;
        private readonly IRepository<Schedule> _scheduleRepository;
        private readonly IRepository<ScheduleDay> _scheduleDayRepository;
        private readonly IRepository<ScheduleItemType> _scheduleItemTypeRepository;
        private readonly IRepository<DAL.Entities.Group> _groupRepository;
        private readonly IGroupService _groupService;

        public ScheduleService(IRepository<ScheduleItem> scheduleItemRepository, 
                               IRepository<Schedule> scheduleRepository, 
                               IRepository<ScheduleDay> scheduleDayRepository, 
                               IRepository<ScheduleItemType> scheduleItemTypeRepository, 
                               IRepository<DAL.Entities.Group> groupRepository, 
                               IGroupService groupService)
        {
            _scheduleItemRepository = scheduleItemRepository;
            _scheduleRepository = scheduleRepository;
            _scheduleDayRepository = scheduleDayRepository;
            _scheduleItemTypeRepository = scheduleItemTypeRepository;
            _groupRepository = groupRepository;
            _groupService = groupService;
        }

        public async Task<List<ScheduleItemViewModel>> GetScheduleItemsAsync(int groupId, int dayId)
        {
            var schedule = await _scheduleRepository.GetQueryable(x=>x.GroupId == groupId && x.DayId == dayId).Include(x=>x.Day).Include(x=> x.ScheduleItems).ThenInclude(x=>x.ScheduleItemType).Include(x=>x.ScheduleItems).ThenInclude(x=>x.Subject).FirstOrDefaultAsync();

            if (schedule == null)
            {
                _scheduleRepository.Add(new Schedule
                {
                    DayId = dayId,
                    GroupId = groupId,
                });

                return new List<ScheduleItemViewModel>();
            }
               


            var viewModel = schedule.ScheduleItems.Select(si => new ScheduleItemViewModel
            {
                Id = si.Id,
                Position = si.Position,
                Subject = si.Subject.Name,
                Url = si.OnlineMeetingUrl,
                Start = si.Start,
                End = si.End,
                Comment = si.Comment,
                SubjectShortInfo = new SubjectShortInfoVM
                {
                    Id = si.SubjectId,
                    Name = si.Subject.Name
                },
                ScheduleDay = new ScheduleDayViewModel
                {
                    Name = schedule.Day.Name,
                    Id = schedule.DayId
                },
                ScheduleItemType = new ScheduleItemTypeViewModel
                {
                    Id = si.ScheduleItemTypeId,
                    Name = si.ScheduleItemType.Name
                }
            }).ToList();

            return viewModel;
        }

        public async Task<List<ScheduleDayViewModel>> GetScheduleDaysAsync()
        {
            var scheduleDays = await _scheduleDayRepository.GetAsync();
            var viewModel = scheduleDays.Select(sd => new ScheduleDayViewModel
            {
                Id = sd.Id,
                Name = sd.Name
            }).ToList();

            return viewModel;
        }

        public async Task<List<ScheduleItemTypeViewModel>> GetScheduleItemTypesAsync()
        {
            var scheduleItemTypes = await _scheduleItemTypeRepository.GetAsync();
            var viewModel = scheduleItemTypes.Select(sit => new ScheduleItemTypeViewModel
            {
                Id = sit.Id,
                Name = sit.Name
            }).ToList();

            return viewModel;
        }

        public async Task CreateNewItemAsync(int groupId, int dayId)
        {
            var schedule = await _scheduleRepository.GetQueryable(x => x.GroupId == groupId && x.DayId == dayId).Include(x=>x.ScheduleItems).FirstOrDefaultAsync();

            if (schedule == null)
            {
                throw new CustomHttpException("Invalid day or group id");
            }

            var types = await GetScheduleItemTypesAsync();

            if (types == null && types.Count== 0)
            {
                throw new CustomHttpException("No lecture types found!");
            }


            var subjects = await _groupService.GetGroupSubjects(groupId);

            if (subjects == null && subjects.ToList().Count == 0)
            {
                throw new CustomHttpException("No group subjects found!");
            }

            var itm = schedule.ScheduleItems.OrderByDescending(x => x.Position).FirstOrDefault();

            int pos = 0;

            if (itm != null)
            {
                pos = itm.Position;
            }


            var item = new ScheduleItem()
            {
                Comment = string.Empty,
                End = "10:20",
                Start = "09:00",
                OnlineMeetingUrl = string.Empty,
                ScheduleId = schedule.Id,
                Position =  pos + 1,
                ScheduleItemTypeId = types.FirstOrDefault().Id,
                SubjectId = subjects.FirstOrDefault().Id,
            };

            _scheduleItemRepository.Add(item);
        }

        public async Task RemoveItemAsync(int itemId)
        {
           var item = await _scheduleItemRepository.GetByIdAsync(itemId);

            if (item == null)
            {
                throw new CustomHttpException("Item not found!");
            }

            _scheduleItemRepository.Delete(item);
        }

        public async Task UpdateItemAsync(EditScheduleItemDTO model)
        {
            var item = await _scheduleItemRepository.GetQueryable(x=>x.Id == model.Id).Include(x=>x.Schedule).FirstOrDefaultAsync();

            if (item == null)
            {
                throw new CustomHttpException("Item not found!");
            }

            var type = (await GetScheduleItemTypesAsync()).FirstOrDefault(x=>x.Id == model.ScheduleItemTypeId);

            if (type == null)
            {
                throw new CustomHttpException("No lecture type found!");
            }


            var subject = (await _groupService.GetGroupSubjects(item.Schedule.GroupId)).FirstOrDefault(x=>x.Id == model.SubjectId);

            if (subject == null)
            {
                throw new CustomHttpException("No group subject found!");
            }

            item.Start = model.Start;
            item.End = model.End;
            item.SubjectId = model.SubjectId;
            item.ScheduleItemTypeId = model.ScheduleItemTypeId;
            item.Comment = model.Comment;
            item.OnlineMeetingUrl = model.Url;

            _scheduleItemRepository.Edit(item);
        
        }
    }

}
