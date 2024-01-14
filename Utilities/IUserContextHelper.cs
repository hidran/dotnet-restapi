namespace PmsApi.Utilities
{
    public interface IUserContextHelper
    {
        string GetUserId();
        bool IsAdmin();
    }
}