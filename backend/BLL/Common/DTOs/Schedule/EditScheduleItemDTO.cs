namespace backend.BLL.Common.DTOs.Schedule;

public class EditScheduleItemDTO
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public string Comment { get; set; }
    public int SubjectId { get; set; }
    public int ScheduleItemTypeId { get; set; }
}