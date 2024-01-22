using System.Diagnostics.CodeAnalysis;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data;

[ExcludeFromCodeCoverage]

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<AnimalBooking> AnimalBookings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var decimalProps = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

        foreach (var property in decimalProps)
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }
        
        builder.SeedAnimals();
        builder.SeedUsers();
    }


}
[ExcludeFromCodeCoverage]
public static class ModelBuilderExtensions
{

    public static void SeedAnimals(this ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Animal>().HasData(
            new Animal()
            {
                Id = 1,
                Name = "Aap",
                Type = AnimalTypes.Jungle.ToString(),
                Price = (decimal)4.50,
                ImageUrl = "/images/animals/aap.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 2,
                Name = "Olifant",
                Type = AnimalTypes.Jungle.ToString(),
                Price = (decimal)16.50,
                ImageUrl = "images/animals/olifant.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 3,
                Name = "Zebra",
                Type = AnimalTypes.Jungle.ToString(),
                Price = (decimal)1.50,
                ImageUrl = "images/animals/zebra.png",
                IsVip = false
            },


            new Animal()
            {
                Id = 4,
                Name = "Leeuw",
                Type = AnimalTypes.Jungle.ToString(),
                Price = (decimal)29.50,
                ImageUrl = "images/animals/leeuw.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 5,
                Name = "Hond",
                Type = AnimalTypes.Boerderij.ToString(),
                Price = (decimal)7.50,
                ImageUrl = "images/animals/hond.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 6,
                Name = "Ezel",
                Type = AnimalTypes.Boerderij.ToString(),
                Price = (decimal)30.50,
                ImageUrl = "images/animals/ezel.png",
                IsVip = false
            },


            new Animal()
            {
                Id = 7,
                Name = "Koe",
                Type = AnimalTypes.Boerderij.ToString(),
                Price = (decimal)1.75,
                ImageUrl = "images/animals/koe.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 8,
                Name = "Eend",
                Type = AnimalTypes.Boerderij.ToString(),
                Price = (decimal)0.75,
                ImageUrl = "images/animals/eend.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 9,
                Name = "Kuiken",
                Type = AnimalTypes.Boerderij.ToString(),
                Price = (decimal)3.75,
                ImageUrl = "images/animals/kuiken.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 10,
                Name = "Pinguin",
                Type = AnimalTypes.Sneeuw.ToString(),
                Price = (decimal)40.00,
                ImageUrl = "images/animals/penguin.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 11,
                Name = "IJsbeer",
                Type = AnimalTypes.Sneeuw.ToString(),
                Price = (decimal)11.75,
                ImageUrl = "images/animals/ijsbeer.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 12,
                Name = "Zeehond",
                Type = AnimalTypes.Sneeuw.ToString(),
                Price = (decimal)23.75,
                ImageUrl = "images/animals/zeehond.png",
                IsVip = false
            },

            new Animal()
            {
                Id = 13,
                Name = "Kameel",
                Type = AnimalTypes.Woestijn.ToString(),
                Price = (decimal)55.20,
                ImageUrl = "images/animals/kameel.gif",
                IsVip = false
            },

            new Animal()
            {
                Id = 14,
                Name = "Slang",
                Type = AnimalTypes.Woestijn.ToString(),
                Price = (decimal)11.20,
                ImageUrl = "images/animals/slang.png",
                IsVip = false
            },
            new Animal()
            {
                Id = 15,
                Name = "T-Rex",
                Type = AnimalTypes.Vip.ToString(),
                Price = (decimal)100.00,
                ImageUrl = "images/animals/trex.png",
                IsVip = true
            },
            new Animal()
            {
                Id = 16,
                Name = "Unicorn",
                Type = AnimalTypes.Vip.ToString(),
                Price = (decimal)100.00,
                ImageUrl = "images/animals/unicorn.jpg",
                IsVip = true
            }
        );
    }

    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        
        var password = new PasswordHasher<User>();
        
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole {Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() });


        modelBuilder.Entity<User>().HasData(
            new User()
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@avans.nl",
                UserName = "admin@avans.nl",
                NormalizedEmail = "ADMIN@AVANS.NL",
                NormalizedUserName = "ADMIN@AVANS.NL",
                Address = "Onderwijsboulevard 215",
                MemberCard = MemberCard.Platinum,
                PasswordHash = password.HashPassword(null!,"secret")

            }
        );
        
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210", 
                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
            }
        );
        
    }
}