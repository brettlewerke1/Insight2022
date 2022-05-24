using Insight2022.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using Insight2022.IDbServices;
using Insight2022.DbServices;

namespace Insight2022.Extentions
{
    public static partial class ServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddDatabases(this WebApplicationBuilder builder)
        {
            builder.Configuration.GetSection("AccountDb");
            builder.Services.AddSingleton<IWorkoutDb>(InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("AccountDb")).GetAwaiter().GetResult());

            return builder;

        }
        static async Task<IWorkoutDb> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["AccountContainer"];
            var account = configurationSection["Url"];
            var key = configurationSection["Key"];
            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var cosmosDbService = new WorkoutDb(client, databaseName, containerName);
            return cosmosDbService;
        }
    }
}
