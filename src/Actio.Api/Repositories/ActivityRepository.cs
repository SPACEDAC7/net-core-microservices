using Actio.Api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace Actio.Api.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IMongoDatabase mongoDatabase;

        public ActivityRepository(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
        }

        public async Task AddAsync(Activity activity) => await Collection.InsertOneAsync(activity);

        public async Task<IEnumerable<Activity>> BrowseAsync(Guid userId)
        {
            return await Collection.AsQueryable().Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Activity> GetAsync(Guid id)
        {
            return await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        private IMongoCollection<Activity> Collection
           => this.mongoDatabase.GetCollection<Activity>("Activities");
    }
}
