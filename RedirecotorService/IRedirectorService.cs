namespace RedirectorService
{
    public interface IRedirectorService
    {
        Task<string> CheckForRedirect(string redirectUrl);
    }
}
