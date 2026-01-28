namespace Movies.Application.Models;

public class Director
{
    public int Id { get; set; }
    public String FullName {get;}

    public String? DateOfBirth {get;}
    
    public List<MovieDirector> Movie_Director { get; set; }
}