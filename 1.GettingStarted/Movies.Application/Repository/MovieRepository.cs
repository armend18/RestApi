using System.Text;
using Dapper;

using Movies.Application.Models;

namespace Movies.Application.Repository;

public class MovieRepository : IMovieRepository
{
    



public async Task<bool> CreateAsync(Movie movie)
{
    // using var connection = await _dbConnectionFactory.CreateConnectionAsync();
    //
    // var result = await connection.ExecuteAsync(new CommandDefinition("""
    //                                                                      INSERT INTO movies (
    //                                                                          id, title, slug, description, year_of_release, run_time, age,
    //                                                                          castList, genres, rating, country, director, cover, video_link
    //                                                                      )
    //                                                                      VALUES (
    //                                                                          @Id, @Title, @Slug, @Description, @YearOfRelease, @RunTime, @Age,
    //                                                                          @CastList, @Genres, @Rating, @Country, @Director, @Cover, @VideoLink
    //                                                                      );
    //                                                                  """, movie
    // ));
    //

    return false;
}

public async Task<Movie?> GetByIdAsync(Guid id)
{
    Movie movie=new Movie
    {
        Id = default,
        Title = null,
        Description = null,
        YearOfRelease = 0,
        RunTime = 0,
        Age = 0,
        Country = null,
        Director = null,
        CastList = new string[]
        {
        },
        Genres = new string[]
        {
        },
        Rating = 0,
        Cover = null,
        VideoLink = null
    };
    // using var connection = await _dbConnectionFactory.CreateConnectionAsync();
    // var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition("""
    //     select * from movies where id = @Id
    //     """, new { Id = id }));
    //
    //
    // if (movie is null)
    // {
    //     return null;
    // }
    return movie ;
}

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        Movie movie=new Movie
        {
            Id = default,
            Title = null,
            Description = null,
            YearOfRelease = 0,
            RunTime = 0,
            Age = 0,
            Country = null,
            Director = null,
            CastList = new string[]
            {
            },
            Genres = new string[]
            {
            },
            Rating = 0,
            Cover = null,
            VideoLink = null
        };
        // using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        // var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition("""
        //     select * from movies where slug = @slug
        //     """, new { slug= slug }));
        //
        //
        // if (movie is null)
        // {
        //     return null;
        // }
        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
    //     using var connection = await _dbConnectionFactory.CreateConnectionAsync();
    //
    //     var movies = await connection.QueryAsync<Movie>(@"
    //     SELECT * FROM movies
    //     ORDER BY year_of_release DESC
    // ")
    
       var movies = new List<Movie>();
        return movies;
    }


    public async Task<bool> UpdateAsync(Movie movie)
    {
    //     using var connection = await _dbConnectionFactory.CreateConnectionAsync();
    //
    //     var result = await connection.ExecuteAsync(@"
    //     UPDATE movies
    //     SET
    //         title = @Title,
    //         description = @Description,
    //         year_of_release = @YearOfRelease,
    //         run_time = @RunTime,
    //         age = @Age,
    //         castlist = @CastList,
    //         genres = @Genres,
    //         rating = @Rating,
    //         country = @Country,
    //         director = @Director,
    //         cover = @Cover,
    //         video_link = @VideoLink
    //     WHERE id = @Id
    // ", movie);

        return false;
    }


public async Task<bool> DeleteByIdAsync(Guid id)
{
    // using var connection = await _dbConnectionFactory.CreateConnectionAsync();
    //
    // var result = await connection.ExecuteAsync(@"
    //     DELETE FROM movies
    //     WHERE id = @Id
    // ", new { Id = id });

    return false;
}



public async Task<bool> ExistsByIdAsync(Guid id)
    {
        // using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        // return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
        //                                                                        select count(1) from movies where id=@id
        //                                                                        """, new { id }));
        //
        //
        return false;
    }
}