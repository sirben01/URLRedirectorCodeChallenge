namespace RedirectorService
{
    public interface IRedirectorService
    {
        Task<(string, int)> CheckForRedirect(string redirectUrl);
    }
}
