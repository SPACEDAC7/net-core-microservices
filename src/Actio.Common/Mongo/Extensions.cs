using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Mongo
{
    public static class Extensions
    {

        public static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {

            Console.WriteLine("We are in add mongoDb");
            services.Configure<MongoOptions>(configuration.GetSection("mongo"));
            services.AddSingleton<MongoClient>(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();

                Console.WriteLine($"Options -> {options.Value.ConnectionString}");

                return new MongoClient(options.Value.ConnectionString);
            });

            services.AddScoped<IMongoDatabase>(c => {
                var options = c.GetService<IOptions<MongoOptions>>();
                var client = c.GetService<MongoClient>();

                return client.GetDatabase(options.Value.Database);
            });

            services.AddScoped<IDatabaseInitializer, MongoInitializer>();
            services.AddScoped<IDatabaseSeeder, MongoSeeder>();
        }
    }
}
