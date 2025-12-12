using System.Text;
using Dapper;

using Movies.Application.Models;

namespace Movies.Application.Repository;

public class MovieRepository : IMovieRepository
{
    
   
private readonly IDbConnectionFactory _dbConnectionFactory;

public MovieRepository(IDbConnectionFactory dbConnectionFactory)
{
    _dbConnectionFactory = dbConnectionFactory;
}


public async Task<bool> CreateAsync(Movie movie)
{
    using var connection = await _dbConnectionFactory.CreateConnectionAsync();

    var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         INSERT INTO movies (
                                                                             id, title, slug, description, year_of_release, run_time, age,
                                                                             castList, genres, rating, country, director, cover, video_link
                                                                         )
                                                                         VALUES (
                                                                             @Id, @Title, @Slug, @Description, @YearOfRelease, @RunTime, @Age,
                                                                             @CastList, @Genres, @Rating, @Country, @Director, @Cover, @VideoLink
                                                                         );
                                                                     """, movie
    ));


    return result > 0;
}

public async Task<Movie?> GetByIdAsync(Guid id)
{
    using var connection = await _dbConnectionFactory.CreateConnectionAsync();
    var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition("""
        select * from movies where id = @Id
        """, new { Id = id }));


    if (movie is null)
    {
        return null;
    }
    return movie;
}

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition("""
            select * from movies where slug = @slug
            """, new { slug= slug }));


        if (movie is null)
        {
            return null;
        }
        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var movies = await connection.QueryAsync<Movie>(@"
        SELECT * FROM movies
        ORDER BY year_of_release DESC
    ");

        return movies;
    }


    public async Task<bool> UpdateAsync(Movie movie)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var result = await connection.ExecuteAsync(@"
        UPDATE movies
        SET
            title = @Title,
            description = @Description,
            year_of_release = @YearOfRelease,
            run_time = @RunTime,
            age = @Age,
            castlist = @CastList,
            genres = @Genres,
            rating = @Rating,
            country = @Country,
            director = @Director,
            cover = @Cover,
            video_link = @VideoLink
        WHERE id = @Id
    ", movie);

        return result > 0;
    }


public async Task<bool> DeleteByIdAsync(Guid id)
{
    using var connection = await _dbConnectionFactory.CreateConnectionAsync();

    var result = await connection.ExecuteAsync(@"
        DELETE FROM movies
        WHERE id = @Id
    ", new { Id = id });

    return result > 0;
}



public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from movies where id=@id
                                                                               """, new { id }));

        
    }
}