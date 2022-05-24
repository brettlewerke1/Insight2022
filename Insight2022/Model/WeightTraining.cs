using Newtonsoft.Json;

namespace Insight2022.Model
{
    public class WeightTraining
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "Default Firstname";

        [JsonProperty(PropertyName = "lastname")]
        public string Lastname { get; set; } = "Default Lastname";

        [JsonProperty(PropertyName = "workoutname")]
        public string WorkoutName { get; set; } = "Default workout";

        [JsonProperty(PropertyName = "workoutkeyword")]
        public string WorkoutKeyWord { get; set; } = "Default keyword";

        [JsonProperty(PropertyName = "workoutmuscle")]
        public string WorkoutMuscle { get; set; } = "Default muscle keyword";

        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; set; } = 0;

        [JsonProperty(PropertyName = "reps")]
        public int Reps { get; set; } = 0;

        [JsonProperty(PropertyName = "sets")]
        public int Sets { get; set; } = 0;

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; } = "Default date";




    }
}
