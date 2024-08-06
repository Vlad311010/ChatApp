namespace ChatApp.Client.Services
{
    public class ClientSideEvents
    {
        public event Action? onChatJoin;
        public void UserHasJoinedChat()
        {
            if (onChatJoin != null)
            {
                onChatJoin();
            }
        }
    }
}
