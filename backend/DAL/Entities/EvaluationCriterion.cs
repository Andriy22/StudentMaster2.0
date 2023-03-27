namespace backend.DAL.Entities;

public class EvaluationCriterion
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MaxGrade { get; set; }
    public bool IsRequired { get; set; }
    public bool IsDeleted { get; set; }
    public int WorkId { get; set; }
}