using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Client.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        public int ChatId { get; set; }
        public ChatGroup Chat { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        
        [Column(TypeName = "VARCHAR(255)")]
        public string Content { get; set; } = string.Empty;

        public Message() { }
        public Message(int userId, int chatId, string content) 
        {
            UserId = userId;
            ChatId = chatId;
            Content = content;
            CreatedAt = DateTime.Now;
        }

        public Message(int userId, int chatId, string content, DateTime dateTime)
        {
            UserId = userId;
            ChatId = chatId;
            Content = content;
            CreatedAt = dateTime;
        }
    }
}
