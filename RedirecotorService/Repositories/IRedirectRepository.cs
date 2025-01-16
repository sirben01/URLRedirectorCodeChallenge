using RedirecotorService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedirecotorService.Repositories
{
    public interface IRedirectRepository
    {
        Task<IEnumerable<RedirectModel>> GetAllAsync();
    }
}
