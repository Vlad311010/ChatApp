using ChatApp.Client.Pages;

namespace ChatApp.Client.Models
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
        public int OwnerId { get; set; }

        public virtual ICollection<ChatGroupMembers> Memebers { get; set; }

        public override bool Equals(object? obj)
        {
            ChatGroup? other = obj as ChatGroup;
            if (other == null)
                return false;

            return other.Id == Id && other.Name == Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ (Name.GetHashCode() * 41);
        }
    }
}
