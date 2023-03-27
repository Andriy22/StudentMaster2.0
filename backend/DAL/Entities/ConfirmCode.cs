using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DAL.Entities;

public class ConfirmCode
{
    public int Id { get; set; }
    public string UserID { get; set; }

    [ForeignKey("UserID")] public User user { get; set; }

    public int Code { get; set; }
    public DateTime CreationTime { get; set; }
    public bool IsUsed { get; set; }
}