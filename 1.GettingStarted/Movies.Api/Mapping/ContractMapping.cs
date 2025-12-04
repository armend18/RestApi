using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping;

public static class ContractMapping
{
   

    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        return new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            YearOfRelease = request.YearOfRelease,
            RunTime = request.RunTime,
            Age = request.Age,
            Director = request.Director,
            Country = request.Country,
            Cast = request.Cast.ToList(),
            Genres = request.Genres.ToList(),
            Rating = request.Rating,
            Cover = request.Cover,
            VideoLink = request.VideoLink
            
        };
    }

    public static MovieResponse MapToResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            YearOfRelease = movie.YearOfRelease,
            RunTime = movie.RunTime,
            Age = movie.Age,
            Director = movie.Director,
            Country = movie.Country,
            Cast = movie.Cast.ToList(),
            Genres = movie.Genres.ToList(),
            Rating =movie.Rating,
            Cover = movie.Cover,
            VideoLink = movie.VideoLink,
        };
    }

    public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies)
    {
        return new MoviesResponse
        {
            Items = movies.Select(MapToResponse)
        };
    }
    public static Movie MapToMovie(this UpdateMovieRequest request,Guid id)
    {
        return new Movie
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            YearOfRelease = request.YearOfRelease,
            RunTime = request.RunTime,
            Age = request.Age,
            Director = request.Director,
            Country = request.Country,
            Cast = request.Cast.ToList(),
            Genres = request.Genres.ToList(),
            Rating = request.Rating,
            Cover = request.Cover,
            VideoLink = request.VideoLink
            
            
        };
    }
    
}