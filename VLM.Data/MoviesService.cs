using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLM.Core.Entities;

namespace VLM.Data
{
    public class MoviesService : IMoviesService
    {
        private readonly VLMDbContext db;

        public MoviesService(VLMDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Movies>> GetAllMoviesAsync(string lang=null, string genre = null, int year = 0)
        {
            IQueryable<Movies> query = db.Movies;
            if(genre != null) query = query.Where(g => g.Genre == genre);
            if(lang != null) query = query.Where(l => l.Language == lang);
            if(year > 0) query = query.Where(y => y.ReleaseYear == year);

            return await query.ToListAsync();
        }
        


        public async Task<Movies> GetMovieByTitleAsync(string movieTitle)
        {
            return await db.Movies.Where(m => m.Title.Equals(movieTitle)).FirstOrDefaultAsync();
        }

        public async Task<Movies> GetMovieByIdAsync(int movieId)
        {
            return await db.Movies.Where(m => m.MoviesId.Equals(movieId)).FirstOrDefaultAsync();
        }

        public async Task<Movies> AddMovieAsync(Movies movieData)
        {
            var movieExistByTitle = await GetMovieByTitleAsync(movieData.Title);
            if (movieExistByTitle != null)
                return null;
            await db.Movies.AddAsync(movieData);
            return movieData;
        }

        public async Task<bool> DeleteMovieAsync(string movieTitle)
        {
            var movie = await GetMovieByTitleAsync(movieTitle);
            if (movie == null) return false;
            db.Movies.Remove(movie);
            return true;
        }

        public async Task<Movies> EditMovieAsync(Movies editMovieData)
        {
            var existedMovie = await GetMovieByTitleAsync(editMovieData.Title);
            if(existedMovie != null)
            { return null; }
            editMovieData.MoviesId = existedMovie.MoviesId;
            db.Entry(existedMovie).State = EntityState.Detached;
            var entity = db.Movies.Update(editMovieData);
            entity.State = EntityState.Modified;
            return editMovieData;
        }

        public async Task<int> CommitAsync()
        {
            return (await db.SaveChangesAsync());
        }

    }
}
