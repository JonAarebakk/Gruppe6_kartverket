using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.EntityFrameworkCore;


// This file is used to set up the connection to the database
namespace Gruppe6_Kartverket.Mvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<GeoChange> GeoChanges { get; set; }
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
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserTypeNavigation)
                .WithMany()
                .HasForeignKey(u => u.UserType);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserInfo)
                .WithOne(ui => ui.User)
                .HasForeignKey<UserInfo>(ui => ui.UserId);

            modelBuilder.Entity<CaseRecord>()
                .HasOne(c => c.CaseLocation)
                .WithMany()
                .HasForeignKey(c => c.LocationId);

            modelBuilder.Entity<CaseRecord>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);



            // Ensures that CaseLocation, CaseRecord, UserInfo and Usertypes have a primary key each
            modelBuilder.Entity<CaseLocation>()
                .HasKey(cl => cl.LocationId);

            
            modelBuilder.Entity<CaseRecord>()
                .HasKey(c => c.CaseRecordId);

            
            modelBuilder.Entity<UserInfo>()
                .HasKey(ui => ui.UserId);

            
            modelBuilder.Entity<UserTypes>()
                .HasKey(ut => ut.UserType);

            base.OnModelCreating(modelBuilder);
        }
    }
}