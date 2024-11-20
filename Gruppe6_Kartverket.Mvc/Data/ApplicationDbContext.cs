using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Gruppe6_Kartverket.Mvc.Data
{
    // This is your ApplicationDbContext class that configures the connection to the database and models
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        // Constructor that accepts DbContextOptions and passes them to the base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for your application entities
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<CaseLocation> CaseLocations { get; set; }
        public DbSet<CaseRecord> CaseRecords { get; set; }

        // Configures the connection to the database with retry on failure
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "Server=mariadb; Port=3306; Database=kartverket_tables; Uid=root; Pwd=123;",
                new MySqlServerVersion(new Version(8, 0, 21)),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(30), 
                    errorNumbersToAdd: null
                )
            );
        }

        // Configures the relationships between tables and models
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Always call base.OnModelCreating to ensure Identity configurations are applied

            // Explicitly define composite primary key for IdentityUserLogin<string>
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                // Define composite key: UserId, LoginProvider, and ProviderKey
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.ProviderKey });
            });

            // Other model configurations
            modelBuilder.Entity<CaseLocation>(entity =>
            {
                entity.Property(e => e.GeoJSON).HasColumnType("json"); // Set the column type for GeoJSON
            });

            modelBuilder.Entity<CaseRecord>()
                .HasOne(cr => cr.CaseLocation) // Set up relationship with CaseLocation
                .WithMany(cl => cl.CaseRecords)
                .HasForeignKey(cr => cr.LocationId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserTypeNavigation) // Set up relationship with UserType
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserType);

            modelBuilder.Entity<UserInfo>()
                .HasOne(ui => ui.User) // Set up one-to-one relationship with User
                .WithOne(u => u.UserInfo)
                .HasForeignKey<UserInfo>(ui => ui.UserId);

            modelBuilder.Entity<CaseRecord>()
                .HasOne(cr => cr.User) // Set up relationship with User
                .WithMany(u => u.CaseRecords)
                .HasForeignKey(cr => cr.UserId);

            // Configure the enum properties for UserInfo
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.Property(e => e.Gender)
                      .HasConversion<string>(); // Store enum as string

                entity.Property(e => e.UserStatus)
                      .HasConversion<string>(); // Store enum as string
            });

            // Configure the enum properties for UserTypes
            modelBuilder.Entity<UserTypes>(entity =>
            {
                entity.Property(e => e.UserTypeDescription)
                      .HasConversion<string>(); // Store enum as string
            });

            // Configure the enum properties for CaseRecord
            modelBuilder.Entity<CaseRecord>(entity =>
            {
                entity.Property(e => e.CaseStatus)
                      .HasConversion<string>(); // Store enum as string
            });
        }
    }
}
