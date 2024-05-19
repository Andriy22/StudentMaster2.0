using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.DTOs.Tests
{
    public class SendTestToReviewDto
    {
        public int TestId { get; set; }
        public string StudentId { get; set; }

        public List<QuestionAnswerDto> QuestionAnswers { get; set; } 
    }
}
