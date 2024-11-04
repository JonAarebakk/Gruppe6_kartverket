using Microsoft.EntityFrameworkCore;
using Gruppe6_Kartverket.Mvc.Models;
using System.Collections.Generic;


namespace Gruppe6_Kartverket.Mvc.Data
{
    public class ApplicationDbContext : DbContext
    {
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

        public DbSet<GeoChange> GeoChanges { get; set; }
    }
}
