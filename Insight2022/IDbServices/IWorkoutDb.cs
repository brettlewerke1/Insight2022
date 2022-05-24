using Insight2022.Model;

namespace Insight2022.IDbServices
{
    public interface IWorkoutDb
    {
        Task AddWorkoutAsync(WeightTraining weight);

        Task<WeightTraining> GetWorkoutAsync( string id);

        Task<List<WeightTraining>> GetWorkoutsAsync(string queryString);

        Task DeleteWorkoutAsync(string name);

        List<WeightTraining> CalcuateImprovement( List<WeightTraining> list);


    }
}
