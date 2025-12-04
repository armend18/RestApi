using Movies.Application.Models;

namespace Movies.Application.Repository;

public class MovieRepository : IMovieRepository
{
    
    private readonly List<Movie> _movies = new()
    {
      
    new Movie
    {
        Id = Guid.NewGuid(),
        Title = "Inception",
        Description = "A skilled thief is offered a chance to have his past crimes forgiven by implanting another person's idea into a target's subconscious.",
        YearOfRelease = 2010,
        RunTime = 148,
        Age = 13,
        Country = "USA",
        Director = "Christopher Nolan",
        Cast = new List<string> { "Leonardo DiCaprio", "Joseph Gordon-Levitt", "Ellen Page" },
        Genres = new List<string> { "Action", "Sci-Fi", "Thriller" },
        Rating = 8.8f,
        Cover = "https://filmartgallery.com/cdn/shop/files/Inception-Vintage-Movie-Poster-Original.jpg?v=1738912645",
        VideoLink = "https://www.youtube.com/embed/YoHD9XEInc0"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        Title = "The Shawshank Redemption",
        Description = "Two imprisoned men bond over years, finding solace and eventual redemption through acts of common decency.",
        YearOfRelease = 1994,
        RunTime = 142,
        Age = 16,
        Country = "USA",
        Director = "Frank Darabont",
        Cast = new List<string> { "Tim Robbins", "Morgan Freeman", "Bob Gunton" },
        Genres = new List<string> { "Drama" },
        Rating = 3.3f,
        Cover = "https://image.tmdb.org/t/p/original/5OWFF1DhvYVQiX1yUrBE9CQqO5t.jpg",
        VideoLink = "https://www.youtube.com/embed/PLl99DlL6b4"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        Title = "Interstellar",
        Description = "A group of explorers travels through a wormhole in space in an attempt to ensure humanity's survival.",
        YearOfRelease = 2014,
        RunTime = 169,
        Age = 12,
        Country = "USA",
        Director = "Christopher Nolan",
        Cast = new List<string> { "Matthew McConaughey", "Anne Hathaway", "Jessica Chastain" },
        Genres = new List<string> { "Adventure", "Drama", "Sci-Fi" },
        Rating = 8.6f,
        Cover = "https://m.media-amazon.com/images/I/61ASebTsLpL._AC_UF1000,1000_QL80_.jpg",
        VideoLink = "https://www.youtube.com/embed/zSWdZVtXT7E"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        Title = "The Dark Knight",
        Description = "Batman faces the Joker, a criminal mastermind who plunges Gotham City into chaos.",
        YearOfRelease = 2008,
        RunTime = 152,
        Age = 13,
        Country = "USA",
        Director = "Christopher Nolan",
        Cast = new List<string> { "Christian Bale", "Heath Ledger", "Aaron Eckhart" },
        Genres = new List<string> { "Action", "Crime", "Drama" },
        Rating = 9.0f,
        Cover = "https://storage.googleapis.com/pod_public/750/257216.jpg",
        VideoLink = "https://www.youtube.com/embed/EXeTwQWrcwY"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        Title = "Parasite",
        Description = "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.",
        YearOfRelease = 2019,
        RunTime = 132,
        Age = 16,
        Country = "South Korea",
        Director = "Bong Joon Ho",
        Cast = new List<string> { "Song Kang-ho", "Lee Sun-kyun", "Cho Yeo-jeong" },
        Genres = new List<string> { "Comedy", "Drama", "Thriller" },
        Rating = 8.6f,
        Cover = "https://image.tmdb.org/t/p/original/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg",
        VideoLink = "https://www.youtube.com/embed/5xH0HfJHsaY"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        Title = "Gladiator",
        Description = "A betrayed Roman general seeks revenge against the corrupt emperor who murdered his family and sent him into slavery.",
        YearOfRelease = 2000,
        RunTime = 155,
        Age = 16,
        Country = "USA",
        Director = "Ridley Scott",
        Cast = new List<string> { "Russell Crowe", "Joaquin Phoenix", "Connie Nielsen" },
        Genres = new List<string> { "Action", "Adventure", "Drama" },
        Rating = 8.5f,
        Cover = "https://i.ebayimg.com/images/g/OcEAAOSwHgdmFp-N/s-l1200.jpg",
        VideoLink = "https://www.youtube.com/embed/P5ieIbInFpg"
    }
    };



    
    public Task<bool> CreateAsync(Movie movie)
    {
       _movies.Add(movie);
       return Task.FromResult(true);
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        var movie = _movies.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(movie);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
       var movieIndex = _movies.FindIndex(x => x.Id == movie.Id);
       if (movieIndex == -1)
       {
          return Task.FromResult(false);
       }
       _movies[movieIndex] = movie;
       return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removedCount = _movies.RemoveAll(x => x.Id == id);
        var movieRemoved = removedCount > 0;
        return Task.FromResult(movieRemoved);
    }
}