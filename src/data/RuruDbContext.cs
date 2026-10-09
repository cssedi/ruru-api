using Microsoft.EntityFrameworkCore;
using ruru_api.src.controller;

namespace ruru_api.src.model
{
    public class RuruDbContext : DbContext {
        public RuruDbContext(DbContextOptions<RuruDbContext> options): base(options)
        {
        }
        public DbSet<User> Users {get; set;}
        public DbSet<Workout> Workout {get;set;}
        public DbSet<Exercise> Exercise {get;set;}
        public DbSet<WorkoutExercise> WorkoutExercise {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workout>(entity =>
            {
                entity.HasKey(w => w.Id);

                entity.HasOne(u => u.User)
                    .WithMany(u => u.Workouts)
                    .HasForeignKey(w => w.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<WorkoutExercise>(entity =>
            {
                entity.HasOne(we => we.Workout)
                    .WithMany(w => w.WorkoutExercises)
                    .HasForeignKey(we => we.WorkoutId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(we => we.Exercise)
                    .WithMany(e => e.WorkoutExercise)
                    .HasForeignKey(we => we.ExerciseId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Test",
                Surname = "User",
                Age = 30,
                Weight = 75f,
                Height = 180f
            });
        }
    }
}