using Carter;
using Insight2022.IDbServices;
using Insight2022.Model;

namespace Insight2022.Modules
{
    public class WorkoutModule : ICarterModule
    {
        private readonly IWorkoutDb _workoutDb;
        List<String> workoutKeyWords = new List<String> { "curls","shrugs", "press", "raises", "pull", "push"};
        List<String> muscleKeyWords = new List<String> {"bicep", "tricep", "lat","calf", "chest","traps","quads", "shoulder" };

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //GET THE WORKOUT'S BY NAME
            //
            /// this is a test comment
            app.MapGet("/workout/{name}", async (HttpContext context, IWorkoutDb workout) =>
            {
                if (!context.Request.RouteValues.TryGetValue("name", out var name))
                {
                    context.Response.StatusCode = 404;
                    return Results.BadRequest("Mrrp");
                }
                else
                {
                    List<WeightTraining> weight = await workout.GetWorkoutsAsync($"SELECT * FROM c WHERE c.workoutkeyword='{name}'");
                    if (weight.Count != 0)
                    {
                        return Results.Ok(weight);
                    }
                    return Results.BadRequest("there is nothing here");
                }
            });

            //GET THE WORKOUT'S BY MUSCLE GROUP
            //
            app.MapGet("/muscle/{name}", async (HttpContext context, IWorkoutDb workout) =>
            {
                if (!context.Request.RouteValues.TryGetValue("name", out var name))
                {
                    context.Response.StatusCode = 404;
                    return Results.BadRequest("Mrrp");
                }
                else
                {
                    List<WeightTraining> weight = await workout.GetWorkoutsAsync($"SELECT * FROM c WHERE c.workoutmuscle='{name}'");
                    if (weight.Count != 0)
                    {
                        return Results.Ok(weight);
                    }
                    return Results.BadRequest("there is nothing here");
                }
            });

            //GET THE WORKOUT'D BY NAME
            app.MapGet("/workout/{name}/weekly", async (HttpContext context, IWorkoutDb workout) =>
            {
                // initialize variables for loop
                float lowestWeight = 10000;
                float highestWeight = -1;
                float improvPercent = 0;
                if (!context.Request.RouteValues.TryGetValue("name", out var name))
                {
                    context.Response.StatusCode = 404;
                    return Results.BadRequest("Mrrp");
                }
                else
                {
                    DateTime date = DateTime.Now.AddDays(-7);
                    List<WeightTraining> weight = await workout.GetWorkoutsAsync($"SELECT * FROM c WHERE c.workoutname='{name}'");
                    if (weight.Count != 0)
                    {
                        List<WeightTraining> properTime = new List<WeightTraining>();
                        foreach(WeightTraining withinWeek in weight)
                        {
                            DateTime subtractedTime = DateTime.Parse(withinWeek.Date);
                            if(subtractedTime >= date)
                            {
                                properTime.Add(withinWeek);
                            }
                        }


                        // loop through the list
                        foreach (WeightTraining oneWorkout in properTime)
                        {
                            // find the lowest weight in the week
                            if(oneWorkout.Weight < lowestWeight)
                            {
                                lowestWeight = oneWorkout.Weight;
                            }
                            // find the highest weight in the week
                            if(oneWorkout.Weight > highestWeight)
                            {
                                highestWeight = oneWorkout.Weight;
                            }
                            // calculate the percentage of improvement
                            improvPercent = (highestWeight / lowestWeight) -1;
                        }
                        improvPercent *= 100;
                        return Results.Ok($"You overall weight for {properTime[0].WorkoutName} during the last week has improved by: {improvPercent}%");
                    }
                    return Results.BadRequest("there is nothing here");
                }
            });

            // ADD A NEW WORKOUT
            //
            app.MapPost("/newworkout", async (HttpContext context, IWorkoutDb workout) =>
            {
                var newWorkout = await context.Request.ReadFromJsonAsync<WeightTraining>();
                if (newWorkout == null)
                {
                    throw new ArgumentNullException(nameof(newWorkout));
                }
                else
                {
                    if (newWorkout.Id == null)
                    {
                        Guid guid = Guid.NewGuid();
                        newWorkout.Id = guid.ToString();
                        newWorkout.Date = DateTime.Now.ToString();
                        string[] workoutSplit = newWorkout.WorkoutName.Split(" ");
                        foreach(string keyname in workoutSplit)
                        {
                            if (workoutKeyWords.Contains(keyname))
                            {
                                newWorkout.WorkoutKeyWord = keyname;
                            }
                            else if(muscleKeyWords.Contains(keyname))
                            {
                                newWorkout.WorkoutMuscle = keyname;
                            }
                        }

                    }

                    await workout.AddWorkoutAsync(newWorkout);
                    context.Response.StatusCode = 204;
                    return Results.Ok(newWorkout);
                }
            });

            //DELETE A WORKOUT
            //
            app.MapDelete("/delete/{name}", async (HttpContext context, IWorkoutDb workout) =>
            {
                if (!context.Request.RouteValues.TryGetValue("name", out var name))
                {
                    context.Response.StatusCode = 404;
                    return Results.BadRequest("Mrrp");
                }
                else
                {
                    await workout.DeleteWorkoutAsync(name.ToString());
                    return Results.Ok($"All workouts with the name:{name}... deleted successfully");
                }
            });

        }
    }
}
