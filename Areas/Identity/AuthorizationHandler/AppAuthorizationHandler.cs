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

        // Inject UserManager vào AppAuthorizationHandler
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
                // Kiểm tra xem requirement là loại nào để xử lý
                // Nếu requirement là GenZrequirement 
                // gọi IsGenZ để kiểm tra có đáp ứng không  - nếu đáp ứng
                // thì thiết lập đã kiểm tra và đáp ứng bằng 
                // cách gọi context.Succeed(requirement);

                // Xử lý nếu requirement là GenZrequirement
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
                else // if is OtherRequirement - có xử lý requirement khác nếu cần, làm tương tự
                {

                }
            }

            return Task.CompletedTask;
        }

        // Phương thức kiểm tra user có đáp ứng requirement GenZrequirement
        private bool IsGenZ(ClaimsPrincipal user, object resource, IAuthorizationRequirement requirement)
        {
            // Lấy ngày sinh của User (Identity có cấu hình bảng User có trường)
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
