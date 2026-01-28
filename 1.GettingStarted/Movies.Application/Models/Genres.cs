namespace Movies.Application.Models;

public class Genres
{
    public int GenreId { get; set; }
    public String GenreName {get;}
    public String Description {get;}
    public List<MovieGenres>  Movie_Genres { get; set; }
    
}