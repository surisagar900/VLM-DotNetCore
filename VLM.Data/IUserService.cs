using System.Collections.Generic;
using System.Threading.Tasks;
using VLM.Core.Auth;
using VLM.Core.Entities;

namespace VLM.Data
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetAllUsersAsync(bool showDeleted = false);
        Task<Users> GetUserByUsernameAsync(string username);
        Task<string> CheckUserCredentials(AuthRequest req);

        Task<AuthResponse> AuthenticateAsync(AuthRequest req);
        Task<AuthResponse> AddUserAsync(Users userData);
        
        void EditUserAsync(Users editUserData);
        Task<bool> DeleteUserAsync(string username);

        Task<int> CommitAsync();
    }
}
