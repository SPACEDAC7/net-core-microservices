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

        public MongoInitializer(IMongoDatabase database, IOptions<MongoOptions> options)
        {
            this.database = database;
            this.seed = options.Value.Seed;
        }

        public async Task InitializeAsync()
        {
            if (!this.initialized)
            {
                return;
            }
            RegisterConventions();
            this.initialized = true;
            if (!this.seed)
            {
                return;
            }

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
