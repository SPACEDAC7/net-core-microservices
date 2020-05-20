using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Actio.Common.Mongo
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private bool initialized;
        private readonly bool seed;
        private readonly IMongoDatabase database;

        public readonly IDatabaseSeeder databaseSeeder;

        public MongoInitializer(IMongoDatabase database, IDatabaseSeeder databaseSeeder, IOptions<MongoOptions> options)
        {
            this.database = database;
            this.databaseSeeder = databaseSeeder;
            this.seed = options.Value.Seed;
        }

        public async Task InitializeAsync()
        {
            Console.WriteLine($"InitializeAsync : {this.initialized}");
            if (this.initialized)
            {
                return;
            }
            RegisterConventions();
            this.initialized = true;
            if (!this.seed)
            {
                return;
            }

            await this.databaseSeeder.SeedAsync();

        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("ActionConventions", new MongoConvention(), x => true);
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention> { 
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(MongoDB.Bson.BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}
