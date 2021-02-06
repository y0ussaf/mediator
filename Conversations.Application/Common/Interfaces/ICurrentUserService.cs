using System.Security.Claims;

namespace Conversations.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public string GetCurrentUserId();
        public ClaimsPrincipal GetClaimsPrincipal();
    }
}