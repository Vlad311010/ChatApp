using Microsoft.AspNetCore.Authorization;

namespace app.Authorization
{
    public class OwnerRequirement : IAuthorizationRequirement 
    {
        public int OwnerId { get; }

        public OwnerRequirement(int userId)
        {
            this.OwnerId = userId;
        }
    }

    public class OwnerAuthorizationHandler : AuthorizationHandler<OwnerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerRequirement requirement)
        {
            string userId = context.User.FindFirst("UserId")!.Value;

            if (userId != null && userId == requirement.OwnerId.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
