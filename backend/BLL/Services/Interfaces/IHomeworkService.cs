using backend.BLL.Common.DTOs.Homework;
using backend.BLL.Common.VMs.Homework;

namespace backend.BLL.Services.Interfaces
{
    public interface IHomeworkService
    {
        Task<List<HomeworkViewModel>> GetHomeworksAsync(int subjectId, int groupId);
        Task<HomeworkViewModel> GetHomeworkAsync(int homeworkId);
        Task CreateHomeworkAsync(CreateHomeworkDto dto);
        Task UpdateHomeworkAsync(UpdateHomeworkDto dto);
        Task DeleteHomeworkAsync(int homeworkId);
        Task SendHomeworkToReviewAsync(SendHomeworkToReviewDto dto);
        Task<List<HomeworkStudentViewModel>> GetHomeworkStudentAsync(int homeworkId, string studentId);
        Task<List<HomeworkStudentViewModel>> GetHomeworksForReviewAsync(int homeworkId);
        Task ReviewHomeworkAsync(ReviewHomeworkDto dto);
        Task CancelHomeworkToReview(int id, string studentId);
    }
}
