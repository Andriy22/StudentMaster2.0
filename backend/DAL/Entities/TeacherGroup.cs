namespace backend.DAL.Entities;

public class TeacherGroup
{
    public string UserId { get; set; }
    public User User { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; }
}