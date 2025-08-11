namespace Pagamentos.API.Authentication
{
    public interface IAppIdentityUser
    {
        string GetUsername();
        Guid GetUserId();
        bool IsAuthenticated();
        bool IsInRole(string role);
        string GetRemoteIpAddress();
        string GetLocalIpAddress();
    }
}
