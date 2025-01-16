namespace RedirecotorService
{
    public interface IRedirectorService
    {
        Task<string> CheckForRedirect(string redirectUrl);
    }
}
