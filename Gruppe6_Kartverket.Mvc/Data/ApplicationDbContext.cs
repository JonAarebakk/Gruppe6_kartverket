using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Gruppe6_Kartverket.Mvc.Models.Database;


// This file is used to set up the connection to the database
namespace Gruppe6_Kartverket.Mvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<CaseLocation> CaseLocations { get; set; }
        public DbSet<CaseRecord> CaseRecords { get; set; }



        // Configures the connection to the database with a retry function
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "Server=localhost;Port=3306;Database=kartverket_tables;User=root;Password=Mnarild12!;SslMode=Preferred;",
                new MySqlServerVersion(new Version(8, 0, 21)),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(30), 
                    errorNumbersToAdd: null 
                )
            );
        }


        // Ensures that the relationships between the tables are set up correctly
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaseLocation>(entity =>
            {
                entity.Property(e => e.GeoJSON).HasColumnType("json");
            });

            modelBuilder.Entity<CaseRecord>()
                .HasOne(cr => cr.CaseLocation)
                .WithMany(cl => cl.CaseRecords)
                .HasForeignKey(cr => cr.LocationId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserTypeNavigation)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserType);

            modelBuilder.Entity<UserInfo>()
                .HasOne(ui => ui.User)
                .WithOne(u => u.UserInfo)
                .HasForeignKey<UserInfo>(ui => ui.UserId);

            modelBuilder.Entity<CaseRecord>()
                .HasOne(cr => cr.User)
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