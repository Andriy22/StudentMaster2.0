﻿using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DAL.Entities;

public class Work
{
    public Work()
    {
        EvaluationCriteria = new HashSet<EvaluationCriterion>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int GroupId { get; set; }
    public int SubjectId { get; set; }

    [ForeignKey("GroupId")] public Group Group { get; set; }

    [ForeignKey("SubjectId")] public Subject Subject { get; set; }

    public ICollection<EvaluationCriterion> EvaluationCriteria { get; set; }
    public ICollection<Grade> Grades { get; set; }
}