using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.EntityFrameworkCore;



namespace Gruppe6_Kartverket.Mvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<GeoChange> GeoChanges { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<CaseLocation> CaseLocations { get; set; }
        public DbSet<Case> Cases { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "Server=localhost;Port=3306;Database=kartverket_tables;User=root;Password=Mnarild12!;SslMode=Preferred;",
                new MySqlServerVersion(new Version(8, 0, 21)),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 2, // Number of retry attempts
                    maxRetryDelay: TimeSpan.FromSeconds(30), // Delay between retries
                    errorNumbersToAdd: null // List of error numbers to consider for retry
                )
            );
        }

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

            // Ensure CaseLocation has a primary key
            modelBuilder.Entity<CaseLocation>()
                .HasKey(cl => cl.LocationId);

            // Ensure Case has a primary key
            modelBuilder.Entity<CaseRecord>()
                .HasKey(c => c.CaseId);

            // Ensure UserInfo has a primary key
            modelBuilder.Entity<UserInfo>()
                .HasKey(ui => ui.UserId);

            // Ensure UserTypes has a primary key
            modelBuilder.Entity<UserTypes>()
                .HasKey(ut => ut.UserType);

            base.OnModelCreating(modelBuilder);
        }
    }
}