using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DAL.Entities;

public class Grade
{
    public int Id { get; set; }
    public int Value { get; set; }
    public string StudentId { get; set; }
    public int EvaluationCriterionId { get; set; }

    [ForeignKey("StudentId")] public User Student { get; set; }

    [ForeignKey("EvaluationCriterionId")] public EvaluationCriterion EvaluationCriterion { get; set; }
}