using Microsoft.AspNetCore.Authorization;

namespace PROG6_BeestjeOpJeFeestje.Authorization;

public class AdminAuthorizationHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
    {
        if (context.User.IsInRole("Administrator"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class AdminRequirement : IAuthorizationRequirement
{
}