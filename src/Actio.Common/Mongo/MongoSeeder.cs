﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actio.Common.Mongo
{
    public class MongoSeeder : IDatabaseSeeder
    {

        protected readonly IMongoDatabase database;

        public MongoSeeder(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task SeedAsync()
        {
            Console.WriteLine("Mongo Seeder - send async");
            var collectionCursor = await this.database.ListCollectionsAsync();
            var collections = await collectionCursor.ToListAsync();

            if (collections.Any())
            {
                return;
            }
            await CustomSeedAsync();
        }

        protected virtual async Task CustomSeedAsync()
        {
            Console.WriteLine("Mongo Seeder - custom send async");
            await Task.CompletedTask;
        }
    }
}
