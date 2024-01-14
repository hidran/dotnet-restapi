using System.Security.Claims;

namespace PmsApi.Utilities;

public class UserContextHelper : IUserContextHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public bool IsAdmin()
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
    }
    public string GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}

