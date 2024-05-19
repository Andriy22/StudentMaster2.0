namespace backend.BLL.Common.DTOs.Tests
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Points { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}