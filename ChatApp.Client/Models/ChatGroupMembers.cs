
using System.Diagnostics.CodeAnalysis;

namespace ChatApp.Client.Models
{
    public class ChatGroupMembers
    {
        public int UserId { get; set; }
        // public User User { get; set; } = default!;
        public int ChatGroupId { get; set; }
        // public ChatGroup ChatGroup { get; set; } = default!;
        public ChatGroupMembers() { }
        public ChatGroupMembers(ChatGroup chatGroup, User user) 
        { 
            ChatGroupId = chatGroup.Id;
            UserId = user.Id;

            //ChatGroup = chatGroup;
            // User = user;
        }

        public bool Equals(ChatGroupMembers other)
        {
            return other.UserId == UserId && other.ChatGroupId == ChatGroupId;
        }

        public override bool Equals(object? obj)
        {
            ChatGroupMembers? other = obj as ChatGroupMembers;
            if (other == null)
                return false;

            return other.UserId == UserId && other.ChatGroupId == ChatGroupId;
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode() ^ (ChatGroupId.GetHashCode() * 67);
        }

    }
}
