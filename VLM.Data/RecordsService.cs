using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLM.Core.Entities;
using VLM.Core.Models;

namespace VLM.Data
{
    public class RecordsService : IRecordsService
    {
        private readonly VLMDbContext db;
        private readonly IUserService userService;
        private readonly IMoviesService moviesService;

        public RecordsService(VLMDbContext db, IUserService userService, IMoviesService moviesService)
        {
            this.db = db;
            this.userService = userService;
            this.moviesService = moviesService;
        }

        public async Task<Records> AddRecordAsync(RecordEntryModel newRecord)
        {
            var user = await userService.GetUserByUsernameAsync(newRecord.Username);
            var movie = await moviesService.GetMovieByIdAsync(newRecord.MovieId);
            if((user == null) || (movie==null) ) return null;

            var newRecordData = new Records()
            {
                User = user,
                Movies = movie,
                TakenDate  = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(movie.ReturnDays)
            };

            await db.Records.AddAsync(newRecordData);
            return newRecordData;
        }

        public async Task<bool> DeleteRecordAsync(string username, int recordId)
        {
            var userRecord = await GetUserRecordsByIdAsync(username, recordId);
            if(userRecord == null)
                return false;
            userRecord.IsCleared = true;
            db.Entry(userRecord).Property("IsCleared").IsModified = true;
            return true;
        }

        public async Task<IEnumerable<Records>> GetAllRecordsAsync()
        {
            var query = db.Records.Include("User").Include("Movies").ToListAsync();
            return await query;
        }

        public async Task<Records> GetRecordsByIdAsync(int recordId)
        {
            return await db.Records.Include("Movies").Include("User").Where(r => r.RecordsId.Equals(recordId)).FirstOrDefaultAsync();
        }

        public async Task<Records> GetUserRecordsByIdAsync(string username, int recordId)
        {
            return await db.Records.Include("Movies").Include("User").Where(r => r.RecordsId.Equals(recordId)).Where(u => u.User.UserName.Equals(username)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Records>> GetRecordsByUsernameAsync(string username)
        {
            return await db.Records.Include("Movies").Include("User").Where(u => u.User.UserName.Equals(username)).OrderByDescending(r => r.TakenDate).ToListAsync();
        }

        public async Task<int> CommitAsync()
        {
            return await db.SaveChangesAsync();
        }

    }
}
