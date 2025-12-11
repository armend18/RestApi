using Microsoft.EntityFrameworkCore;
using Movies.Application.Models;

namespace Movies.Application.Data;

public class MoviesContext: DbContext
{
    public DbSet<Movie> Movies=> Set<Movie>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Movies;User ID=sa;Pwd=123Nukedi; TrustServerCertificate=true");
        base.OnConfiguring(optionsBuilder);
    }
}