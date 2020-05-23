using Actio.Common.Mongo;
using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Activities.Services
{
    public class CustomIdentityMongoSeeder : MongoSeeder
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<CustomIdentityMongoSeeder> logger;

        public CustomIdentityMongoSeeder(IMongoDatabase database, IUserRepository categoryRepository, ILogger<CustomIdentityMongoSeeder> logger) : base(database)
        {
            Console.WriteLine("CustomMongoSeeder");
            this.userRepository = categoryRepository;
            this.logger = logger;
        }

        protected override async Task CustomSeedAsync()
        {
            this.logger.LogInformation("Doing a custom seed Async");
            Console.WriteLine("Doing a custom seed Async");

            var categories = new List<string>
            {
                "user1@test",
                "user2@test",
                "user3@test"
            };

            await Task.WhenAll(categories.Select(x => this.userRepository.AddAsync(new User(x, "name"))));
        }
    }
}
