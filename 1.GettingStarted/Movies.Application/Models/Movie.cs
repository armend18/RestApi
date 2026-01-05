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
    
    public required string Country { get; set; }
    
    public required string Director { get; set; }

    public required String[] CastList { get; init; } = Array.Empty<string>();
    
    public required String[] Genres { get; init; } = Array.Empty<string>();

    public required float Rating { get; set; } = 0;
    
    public required string Cover{get;set;}
    
    public required string VideoLink{get;init;}
    
    public DateOnly DateCreated=DateOnly.FromDateTime(DateTime.Now);
  
    private string GenerateSlug()
    {
        var sluggedTitle=Regex.Replace(Title,"[^0-9A-Za-z _-]",string.Empty).ToLower().Replace(" ","-");
        return $"{sluggedTitle}-{YearOfRelease}";
        
    }
    

}
