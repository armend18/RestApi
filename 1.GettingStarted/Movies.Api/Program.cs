using Microsoft.EntityFrameworkCore;
using Movies.Application;
using Movies.Application.Data;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MoviesContext>();
// builder.Services.AddDatabase(configuration["Database:ConnectionString"]!);
// CORS: Allow frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5501")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

//remove later
var scope=app.Services.CreateScope();
var context=scope.ServiceProvider.GetRequiredService<MoviesContext>();

var pendingMigrations= await context.Database.GetPendingMigrationsAsync();
if (pendingMigrations.Count() > 0)
{
    throw new Exception("Database is not fully migrated.");
}


// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANT: CORS MUST BE BEFORE AUTH and MAPCONTROLLERS
app.UseCors("AllowFrontend");

// --- ERROR WAS HERE ---
// You had app.UseHttpsRedirection(); running here unconditionally.
// I have commented it out so the logic below controls it.
// app.UseHttpsRedirection(); 

// Logic to only use HTTPS redirection in Production
if (app.Environment.IsDevelopment())
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        Console.WriteLine("WARNING: HTTPS Redirection Disabled for Local Testing.");
    });
}
else
{
    // Only redirect to HTTPS if we are NOT in development
    app.UseHttpsRedirection();
}


app.UseAuthorization();
app.MapControllers();

// Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// var dbInitilaizer = app.Services.GetRequiredService<DbInitilaizer>();
// await dbInitilaizer.InitializeAsync();
app.Run();