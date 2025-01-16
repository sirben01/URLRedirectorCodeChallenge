using RedirectorService.Models;

namespace RedirectorService.Repositories
{
    public interface IRedirectRepository
    {
        Task<IEnumerable<RedirectModel>> GetAllAsync();
    }
}
