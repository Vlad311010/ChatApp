using ChatApp.Client.Models;

namespace ChatApp.Interfaces
{
    public interface IChatGroupMember
    {
        // Task<IEnumerable<User>> AllMembers(ChatGroup chatGroup);
        Task<IEnumerable<ChatGroup>> UserChats(User user);
        Task<bool> IsMember(User user, ChatGroup chatGroup);
    }
}
