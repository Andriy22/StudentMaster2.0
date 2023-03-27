namespace backend.DAL.Entities;

public class ScheduleItemType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ScheduleItem> ScheduleItems { get; set; }
}