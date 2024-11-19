using Microsoft.EntityFrameworkCore;
using Gruppe6_Kartverket.Mvc.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework with MariaDB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(10, 5, 9))));

// Register the IDbConnection for Dapper
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the DapperRepository
builder.Services.AddScoped<DapperRepository>();

// The JSON serializer will use the exact property names as defined in your C# classes
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/LandingPage/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LandingPage}/{action=LandingPage}/{id?}");

app.Run();