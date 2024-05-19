using backend.BLL.Common.DTOs.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Interfaces
{
    public interface ITestService
    {
        Task CreateTestAsync(TestDto entity);
        Task DeleteTestAsync(int id);
        Task<List<TestDto>> GetTestsAsync(int subjectId, int groupId);
        Task<TestDto> GetTestAsync(int testId);
        Task<decimal> SendTestToReviewAsync(SendTestToReviewDto entity);
    }
}
