using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using Insight2022.Model;

namespace Insight2022.Contexts
{
    public class WorkoutDbContext : DbContext
    {
        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        { 
            
        }
        public DbSet<WeightTraining>? workouts { get; set; }

    }
}
