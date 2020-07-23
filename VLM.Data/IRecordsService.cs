using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLM.Core.Entities;
using VLM.Core.Models;

namespace VLM.Data
{
    public interface IRecordsService
    {
        Task<IEnumerable<Records>> GetAllRecordsAsync();

        Task<IEnumerable<Records>> GetRecordsByUsernameAsync(string username);

        Task<Records> GetRecordsByIdAsync(int recordId);

        Task<Records> GetUserRecordsByIdAsync(string username, int recordId);

        Task<bool> DeleteRecordAsync(string username, int recordId);
        
        Task<Records> AddRecordAsync(RecordEntryModel newRecord);

        Task<int> CommitAsync();
    }
}
