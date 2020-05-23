using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace Actio.Services.Identity.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly IMongoDatabase database;

        public UserRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task AddAsync(User user) => await Collection.InsertOneAsync(user);

        public async Task<User> GetAsync(Guid id) => await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        public async Task<User> GetAsync(string email) => await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Email == email.ToLowerInvariant());

        private IMongoCollection<User> Collection
            => this.database.GetCollection<User>("Users");
    }
}
