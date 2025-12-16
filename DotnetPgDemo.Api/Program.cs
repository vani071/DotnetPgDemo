using DotnetPgDemo.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

//registring our AppDbContext with dependency injection
string connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found.");
builder.Services.AddDbContext<AppDbContext>(options=>options.UseNpgsql(connectionString));

var app = builder.Build();


app.MapControllers();

app.Run();
