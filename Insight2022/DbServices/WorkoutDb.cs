using Insight2022.IDbServices;
using Insight2022.Model;
using Microsoft.Azure.Cosmos;

namespace Insight2022.DbServices
{
    public class WorkoutDb : IWorkoutDb
    {
        private Container _container;



        public WorkoutDb(CosmosClient dbClient, string Databasename, string containerName)
        {
            this._container = dbClient.GetContainer(Databasename, containerName);
        }

        public async Task AddWorkoutAsync(WeightTraining weight)
        {
            if (weight != null)
            {
                try
                {
                    await this._container.CreateItemAsync<WeightTraining>(weight, new PartitionKey(weight.Id.ToString()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        public async Task<WeightTraining> GetWorkoutAsync(string id)
        {
            try
            {
                ItemResponse<WeightTraining> response = await this._container.ReadItemAsync<WeightTraining>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<WeightTraining>> GetWorkoutsAsync(string queryString)
        {
            using (FeedIterator<WeightTraining> query = this._container.GetItemQueryIterator<WeightTraining>(new QueryDefinition(queryString)))
            {
                List<WeightTraining> list = new List<WeightTraining>();
                while (query.HasMoreResults)
                {
                    FeedResponse<WeightTraining> response = await query.ReadNextAsync();

                    list.AddRange(response.ToList());
                }

                return list;
            }
        }

        public async Task DeleteWorkoutAsync(string name)
        {
            // get the id of the workout by the name
            string queryString = $"SELECT * FROM c WHERE c.workoutname='{name}'";
            List<WeightTraining> list = new List<WeightTraining>();
            using (FeedIterator<WeightTraining> query = this._container.GetItemQueryIterator<WeightTraining>(new QueryDefinition(queryString)))
            {
                while (query.HasMoreResults)
                {
                    FeedResponse<WeightTraining> response = await query.ReadNextAsync();
                    list.AddRange(response.ToList());
                }
            }
            foreach(WeightTraining workout in list)
            {
                await this._container.DeleteItemAsync<WeightTraining>(workout.Id, new PartitionKey(workout.Id));

            }

        }

        public List<WeightTraining> CalcuateImprovement(List<WeightTraining> list)
        {
            foreach (WeightTraining workout in list)
            {
                return null;
            }
            return null;
        }
    }
}
