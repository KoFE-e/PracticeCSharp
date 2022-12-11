using Microsoft.EntityFrameworkCore;
using PracticeCSharp.Domain.Entity;
using PracticeCSharp.Domain.Enum;
using PracticeCSharp.Domain.Helpers;
using System;

namespace PracticeCSharp.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Car> Car { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Profile> Profile { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("User").HasKey(x => x.Id);

                builder.HasData(new User
                {
                    Id = 1,
                    Name = "FirstAdmin",
                    Password = HashPasswordHelper.HashPassword("1234"),
                    Role = Role.Admin
                });


                builder.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
                builder.Property(x => x.Password).IsRequired();

                builder.HasOne(x => x.Profile)
                    .WithOne(x => x.User)
                    .HasPrincipalKey<User>(x => x.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Car>(builder =>
            {
                builder.ToTable("Car").HasKey(x => x.Id);

                builder.HasData(new Car
                {
                    Id = 1,
                    Name = "BMW X5",
                    Description = "Best car",
                    DateCreate = DateTime.Now,
                    Speed = 230,
                    Model = "BMW",
                    Avatar = null,
                    TypeCar = TypeCar.PassengerCar
                });
            });

            modelBuilder.Entity<Profile>(builder =>
            {
                builder.ToTable("Profile").HasKey(x => x.Id);

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.Age);
                builder.Property(x => x.Address).HasMaxLength(200).IsRequired(false);

                builder.HasData(new Profile()
                {
                    Id = 1,
                    UserId = 1
                });
            });
        }
    }
}
