namespace ChatApp.Client.Services
{
    public class ClientSideEvents
    {
        public event Action<string>? onChatGroupJoin;
        public void ChatGroupJoin(string chatName)
        {
            if (onChatGroupJoin != null)
            {
                onChatGroupJoin(chatName);
            }
        }

        public event Action<string>? onChatGroupLeave;
        public void ChatGroupLeave(string chatName)
        {
            if (onChatGroupLeave != null)
            {
                onChatGroupLeave(chatName);
            }
        }
    }
}
