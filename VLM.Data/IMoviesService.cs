using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLM.Core.Entities;

namespace VLM.Data
{
    public interface IMoviesService
    {
        Task<Movies> AddMovieAsync(Movies movieData);
        Task<Movies> EditMovieAsync(Movies editMovieData);
        Task<Movies> GetMovieByTitleAsync(string movieTitle);
        Task<Movies> GetMovieByIdAsync(int movieId);
        Task<bool> DeleteMovieAsync(string movieTitle);
        Task<IEnumerable<Movies>> GetAllMoviesAsync(string lang=null, string genre=null,int year= 0);

        Task<int> CommitAsync();
    }
}
