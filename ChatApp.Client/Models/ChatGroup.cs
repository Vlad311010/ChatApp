namespace ChatApp.Client.Models
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;

        public int OwnerId { get; set; }
        public virtual User Owner { get; set; } = default!;
        public ICollection<User> Participants { get; } = new List<User>(); 
    }
}
