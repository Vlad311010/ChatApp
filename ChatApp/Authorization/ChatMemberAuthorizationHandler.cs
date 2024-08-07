using ChatApp.Client.Models;
using ChatApp.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace app.Authorization
{
    public class ChatMemberRequirement : IAuthorizationRequirement 
    {
        public string chatName { get; }

        public ChatMemberRequirement(string chatName)
        {
            this.chatName = chatName;
        }
    }

    public class ChatMemberAuthorizationHandler : AuthorizationHandler<ChatMemberRequirement>
    {
        private readonly IChatsRepository chatsRepo;

        public ChatMemberAuthorizationHandler(IChatsRepository chatsRepository)
        {
            chatsRepo = chatsRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ChatMemberRequirement requirement)
        {
            ChatGroup? chat = await chatsRepo.GetByName(requirement.chatName, true);

            int userId;
            if (chat == null || !context.User.Identity!.IsAuthenticated || !Int32.TryParse(context.User.FindFirst("UserId")!.Value, out userId))
                return;

            if (chat.Memebers.Contains(new ChatGroupMembers(chat.Id, userId)))
                context.Succeed(requirement);

            return;
        }
    }

}
