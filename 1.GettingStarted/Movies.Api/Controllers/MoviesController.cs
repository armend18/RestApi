using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Mapping;
using Movies.Application.Data;
using Movies.Application.Models;
using Movies.Application.Repository;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;
[ApiController]


public class MoviesController: ControllerBase
{
    private readonly MoviesContext _context;
    
    public MoviesController(MoviesContext context)
        {
        _context = context;
        }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
    }
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out var id) ? await _context.Movies.FirstOrDefaultAsync(m=>m.Id==id) : await _context.Movies.FirstOrDefaultAsync(m=>m.Slug==idOrSlug);
        if (movie is null)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);
    }
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _context.Movies.ToListAsync();
        var moviesResponse = movies.MapToResponse();
        return Ok(moviesResponse);
       
    }
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        
        if (movie is null)
        {
            return NotFound();
        }

        _context.Entry(movie).CurrentValues.SetValues(request);
        await _context.SaveChangesAsync();

        var response= movie.MapToResponse();
        return Ok(response);
    }
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        
        if (movie is null)
        {
            return NotFound();
        }
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet(ApiEndpoints.Movies.GetByGenre)]
    public async Task<IActionResult> GetByGenre([FromRoute] string genre)
    {

        var filteredMovies = await _context.Movies.Where(m => m.Genres.Contains(genre)).ToListAsync();
        return Ok(filteredMovies.MapToResponse());
    }
    
    

}

