using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DAL.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string OwnerId { get; set; }

    [ForeignKey("OwnerId")] public User Owner { get; set; }

    public string SenderId { get; set; }

    [ForeignKey("SenderId")] public User Sender { get; set; }

    public string Color { get; set; }
    public DateTime Date { get; set; }
    public string Room { get; set; }
}