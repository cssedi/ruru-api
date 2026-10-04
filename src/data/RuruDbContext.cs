using Microsoft.EntityFrameworkCore;
using ruru_api.src.controller;

namespace ruru_api.src.model
{
    public class RuruDBContext : DbContext {
        public RuruDBContext(DbContextOptions<RuruDBContext> options): base(options)
        {
        }
        public DbSet<User> Users {get; set;}
        public DbSet<Workout> Workout {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workout>(entity =>
            {
                entity.HasKey(w => w.Id);

                entity.HasOne(u => u.User)
                .WithMany(u=> u.Workouts)
                .HasForeignKey(w=>w.UserId);
                //TODO: configure delete logic
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