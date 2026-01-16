using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movies.Application.Models;

namespace Movies.Application.Data;

public class MoviesContext : IdentityDbContext<User>
{
    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<User> Users=> Set<User>();
    
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
}