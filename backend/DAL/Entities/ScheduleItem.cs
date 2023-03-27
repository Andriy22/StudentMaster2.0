namespace backend.DAL.Entities;

public class ScheduleItem
{
    public int Id { get; set; }
    public string OnlineMeetingUrl { get; set; }
    public int Position { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public string Comment { get; set; }
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public int ScheduleId { get; set; }
    public Schedule Schedule { get; set; }
    public int ScheduleItemTypeId { get; set; }
    public ScheduleItemType ScheduleItemType { get; set; }
}