using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ShopOnline.Models;

namespace ShopOnline.Areas.Identity.AuthorizationHandler
{
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AppAuthorizationHandler> _logger;

        public AppAuthorizationHandler(UserManager<AppUser> userManager, ILogger<AppAuthorizationHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            // lấy các requirement chưa được kiểm tra trong ngữ cảnh xác thực hiện tại
            var pendingRequirements = context.PendingRequirements.ToList();
            foreach (var requirement in pendingRequirements)
            {
                if (requirement is GenZrequirement)
                {
                    if (IsGenZ(context.User, context.Resource, requirement))
                    {
                        _logger.LogInformation("IsGenZ success");
                        context.Succeed(requirement);
                    }
                    else
                    {
                        _logger.LogInformation("IsGenZ false");
                    }
                }
                else // other requirement 
                {

                }
            }

            return Task.CompletedTask;
        }

        
        private bool IsGenZ(ClaimsPrincipal user, object resource, IAuthorizationRequirement requirement)
        {
            var taskgetuser = _userManager.GetUserAsync(user);
            Task.WaitAll(taskgetuser);
            var appuser = taskgetuser.Result;

            if (appuser.Dob == null) return false;
            var require = requirement as GenZrequirement;

            int year = appuser.Dob.Year;
            return (year >= require.MinYear && year <= require.MaxYear);

        }
    }
}
