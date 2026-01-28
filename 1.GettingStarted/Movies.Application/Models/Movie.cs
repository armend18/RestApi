using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public class Movie
{
    public required Guid Id{get;init;}
    public required string Title { get; init; }

    public string Slug => GenerateSlug();


    public required string Description { get; set; }

    public required int YearOfRelease { get; set; }
    
    public required int RunTime { get; set; }
    
    public required int Age{get;set;}
    
    public required string Country { get; set; }// separate table
    
    public List<MovieDirector> Movie_Director{ get; set; }
    
    public List<MovieGenres>  Movie_Genres{ get; set; }
    
    public List<MovieCast>  Movie_Casts{ get; set; }
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    
    
    // public required string Director { get; set; }
    //
    // public required String[] CastList { get; init; } = Array.Empty<string>();//cast on another table
    //
    // public required String[] Genres { get; init; } = Array.Empty<string>();//generes on a diffrent tbale

    public required float Rating { get; set; } = 0;
    
    public required string Cover{get;set;}
    
    public required string VideoLink{get;init;}
    
    public DateOnly DateCreated{ get; set; }=DateOnly.FromDateTime(DateTime.Now);
  
    private string GenerateSlug()
    {
        var sluggedTitle=Regex
            .Replace(Title,"[^0-9A-Za-z _-]",string.Empty).ToLower().Replace(" ","-");
        return $"{sluggedTitle}-{YearOfRelease}";
        
    }
    

}
