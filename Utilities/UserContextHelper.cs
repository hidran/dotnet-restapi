using System.Security.Claims;

namespace PmsApi.Utilities;

    public class UserContextHelper
    {
    private readonly HttpContext _httpContext;

    public UserContextHelper(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }
    public   bool IsAdmin()
    {
       return _httpContext.User.IsInRole("Admin");
    }
    public  string? GetUserId()
    {
        return _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}

