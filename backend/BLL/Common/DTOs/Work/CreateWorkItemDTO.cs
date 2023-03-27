namespace backend.BLL.Common.DTOs.Work;

public class CreateWorkItemDTO
{
    public string Name { get; set; }
    public int MaxGrade { get; set; }
    public bool Removable { get; set; }
}