using Movies.Application;
using Movies.Application.Database;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddDatabase(configuration["Database:ConnectionString"]!);
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
// In your Program.cs / Startup.cs
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var dbInitilaizer = app.Services.GetRequiredService<DbInitilaizer>();
await dbInitilaizer.InitializeAsync();
app.Run();