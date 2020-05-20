using Actio.Common.Mongo;
using Actio.Services.Activities.Domain.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Activities.Services
{
    public class CustomMongoSeeder : MongoSeeder
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ILogger<CustomMongoSeeder> logger;

        public CustomMongoSeeder(IMongoDatabase database, ICategoryRepository categoryRepository, ILogger<CustomMongoSeeder> logger) : base(database)
        {
            Console.WriteLine("CustomMongoSeeder");
            this.categoryRepository = categoryRepository;
            this.logger = logger;
        }

        protected override async Task CustomSeedAsync()
        {
            this.logger.LogInformation("Doing a custom seed Async");
            Console.WriteLine("Doing a custom seed Async");

            var categories = new List<string>
            {
                "work",
                "sport",
                "hobby"
            };

            await Task.WhenAll(categories.Select(x => this.categoryRepository.AddAsync(new Domain.Models.Category(x))));
        }
    }
}
