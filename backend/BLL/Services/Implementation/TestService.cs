using backend.BLL.Common.DTOs.Tests;
using backend.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Implementation
{
    public class TestService : ITestService
    {
        private readonly List<TestDto> _tests = new List<TestDto>();

        public TestService()
        {
            _tests.Add(new TestDto
            {
                Id = 1,
                Name = "Test1",
                Description = "Test1",
                SubjectId = 1,
                GroupId = 1,
                Deadline = DateTime.Now,
                Questions = new List<QuestionDto>
                {
                    new QuestionDto
                    {
                        Text = "Question1",
                        Points = 10,
                        Answers = new List<AnswerDto>
                        {
                            new AnswerDto
                            {
                                Text = "Answer1",
                                IsCorrect = true
                            },
                            new AnswerDto
                            {
                                Text = "Answer2",
                                IsCorrect = false
                            }
                        }
                    }
                }
            });
        }

        public Task CreateTestAsync(TestDto entity)
        {
            entity.Id = _tests.Count + 1;
            _tests.Add(entity);

            return Task.CompletedTask;
        }

        public Task DeleteTestAsync(int id)
        {
            var testToRemove = _tests.FirstOrDefault(x => x.Id == id);

            if (testToRemove == null)
            {
                return Task.CompletedTask;
            }

            _tests.Remove(testToRemove);
            return Task.CompletedTask;
        }

        public Task<TestDto> GetTestAsync(int testId)
        {
            return Task.FromResult(_tests.FirstOrDefault(x => x.Id == testId));
        }

        public Task<List<TestDto>> GetTestsAsync(int subjectId, int groupId)
        {
           return Task.FromResult(_tests.Where(x => x.SubjectId == subjectId && x.GroupId == groupId).ToList());
        }

        public Task<decimal> SendTestToReviewAsync(SendTestToReviewDto entity)
        {
            // we should check if answers are correct and sum points
            
            decimal points = 0;

            foreach (var question in entity.QuestionAnswers)
            {
                var testQuestion = _tests.FirstOrDefault(x => x.Id == entity.TestId).Questions.FirstOrDefault(x => x.Id == question.QuestionId);
                if (testQuestion == null)
                {
                    continue;
                }

                foreach (var answer in question.AnswerIds)
                {
                    var pointsForAnswer = testQuestion.Points / testQuestion.Answers.Where(x => x.IsCorrect).Count();
                    var testAnswer = testQuestion.Answers.FirstOrDefault(x => x.Id == answer);
                    if (testAnswer == null)
                    {
                        continue;
                    }

                    if (testAnswer.IsCorrect)
                    {
                        points += pointsForAnswer;
                    }
                }
            }

            return Task.FromResult(points);
        }
    }
}
