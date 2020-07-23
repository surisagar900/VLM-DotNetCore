using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VLM.Core.Entities;
using VLM.Core.Helpers;
using VLM.Data;

namespace VLM_DotNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly IMoviesService moviesService;
        private readonly LinkGenerator link;

        public MoviesController(IMoviesService moviesService, LinkGenerator link)
        {
            this.moviesService = moviesService;
            this.link = link;
        }

        // GET: all movies
        [HttpGet]
        public async Task<ActionResult<IList<Movies>>> GetAll(string lang = null, string genre = null, int year = 0)
        {
            try
            {
                var movies = await moviesService.GetAllMoviesAsync(lang,genre,year);
                if (movies == null) return NotFound( new { message = "NO_MOVIES" } );
                return Ok(movies);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // GET: movie by title
        [HttpGet("{title}")]
        public async Task<ActionResult<Movies>> GetByMovieTitle(string title)
        {
            try
            {
                var movies = await moviesService.GetMovieByTitleAsync(title);
                if (movies == null) return NotFound(new { message = "MOVIE_NOT_EXIST" } );
                return Ok(movies);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{movieId:int}")]
        public async Task<ActionResult<Movies>> GetMovieById(int movieId)
        {
            try
            {
                var movies = await moviesService.GetMovieByIdAsync(movieId);
                if(movies == null) return NotFound( new { message = "MOVIE_NOT_EXIST" });
                return Ok(movies);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // POST: add new movie
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<Movies>> AddNewMovie(Movies newMovieData)
        {
            try
            {
                var addedMovie = await moviesService.AddMovieAsync(newMovieData);

                if (addedMovie == null)
                {
                    return BadRequest( new { message = "MOVIE_ALREADY_EXIST" } );
                }

                var location = link.GetPathByAction("GetByMovieTitle", "Movies", new { title = newMovieData.Title });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest(new { message = "MOVIETITLE_OCCUPIED" });
                }

                if ((await moviesService.CommitAsync()) > 0)
                {
                    return Created(location, newMovieData);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        // PUT: edit movie by title
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{title}")]
        public async Task<ActionResult<Movies>> EditMovie(string title, Movies editMovieData)
        {
            try
            {
                var editedMovie = await moviesService.EditMovieAsync(editMovieData);
                if(editedMovie == null)
                    return BadRequest(new { message = "MOVIETITLE_OCCUPIED" });
                await moviesService.CommitAsync();
                return Ok(editMovieData);
            }
            catch (Exception)
            {
                //throw;
                return this.StatusCode(StatusCodes.Status405MethodNotAllowed, "Not Allowed, FK Error");
            }
        }

        // Delete: delete movie
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{title}")]
        public async Task<ActionResult<Movies>> DeleteMovie(string title)
        {
            try
            {
                var deletedMovie = await moviesService.DeleteMovieAsync(title);
                if (deletedMovie)
                {
                    await moviesService.CommitAsync();
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status405MethodNotAllowed, "Not Allowed, FK Error");
            }
            return BadRequest();
        }
    }
}
