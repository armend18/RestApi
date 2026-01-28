namespace Movies.Application.Models;

public class Comment
{
    public Guid Id { get; set; }
    

    public required string Text { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;


    public bool IsDeleted { get; set; } = false;

    public required Guid MovieId { get; set; }
   
    public Movie? Movie { get; set; } 


    public required Guid UserId { get; set; }

    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }

    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}