namespace backend.API.Hubs.Models
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public string SenderId { get; set; }
        public string OwnerId { get; set; }
        public string SenderAvatar { get; set; }
        public string Date { get; set; }
        public string SenderFullName { get; set; }
        public string Color { get; set; }
    }
}
