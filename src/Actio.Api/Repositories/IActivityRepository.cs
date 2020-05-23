using Actio.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Api.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity> GetAsync(Guid id);
        Task<IEnumerable<Activity>> BrowseAsync(Guid userId);
        Task AddAsync(Activity activity);
    }
}
