using System.Security.Claims;

namespace QuizPond.Infrastructure
{
    public interface IUserManagerWrapper
    {
        string GetUserIdFromIdentity(ClaimsPrincipal CurrentUser);
    }
}