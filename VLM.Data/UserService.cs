using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VLM.Core.Entities;
using VLM.Core.Auth;
using VLM.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VLM.Core.Models;

namespace VLM.Data
{
    public class UserService : IUserService
    {
        private readonly AppSettings appSettings;
        private readonly VLMDbContext dbContext;

        public UserService(IOptions<AppSettings> appSettings, VLMDbContext dbContext)
        {
            this.appSettings = appSettings.Value;
            this.dbContext = dbContext;
        }

        public async Task<AuthResponse> AddUserAsync(Users userData)
        {
            var exist = await GetUserByUsernameAsync(userData.UserName);
            if (userData == null || exist != null)
                return null;
            await dbContext.Users.AddAsync(userData);
            var token = generateJwtToken(userData.UserName,"User");
            return new AuthResponse(userData.UserName, token);
        }

        public async Task<string> CheckUserCredentials(AuthRequest req)
        {
            string ErrorCode = "";
            if(req.Role == "User")
            {
                Users user = await dbContext.Users.SingleOrDefaultAsync(x => x.UserName == req.Username);
                if(user == null) return ErrorCode = "USERNAME_NOT_FOUND";
                if(user.Password != req.Password) return ErrorCode = "INCORRECT_PASSWORD";
            }
            if(req.Role == "Admin")
            {
                Admin admin = await dbContext.Admin.SingleOrDefaultAsync(x => x.UserName == req.Username);
                if(admin == null) return ErrorCode = "USERNAME_NOT_FOUND";
                if(admin.Password != req.Password) return ErrorCode = "INCORRECT_PASSWORD";
            }
            ErrorCode = "SUCCESS";
            return ErrorCode;
        }


        public async Task<AuthResponse> AuthenticateAsync(AuthRequest req)
        {
            var token = generateJwtToken(req.Username, req.Role);
            return new AuthResponse(req.Username, token);
        }

        public async Task<int> CommitAsync()
        {
            return (await dbContext.SaveChangesAsync());
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user == null)
                return false;
            user.IsActive = false;
            dbContext.Entry(user).Property("IsActive").IsModified = true;
            return true;
        }

        public void EditUserAsync(Users editUserData)
        {
            var existedUser = dbContext.Users.FirstOrDefault(u => u.UserName.Equals(editUserData.UserName));
            if (existedUser != null)
            {
                dbContext.Entry(existedUser).State = EntityState.Detached;
            }
            var entity = dbContext.Users.Update(editUserData);
            entity.State = EntityState.Modified;
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync(bool showDeleted = false)
        {
            if(showDeleted==true)
                return await dbContext.Users.ToListAsync();
            return await dbContext.Users.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            IQueryable<Users> query = from u in dbContext.Users where u.UserName == username && u.IsActive == true select u;
            return await query.FirstOrDefaultAsync();
        }




        ////////// TOKEN GENERATION CODE //////////
        private tokenResponse generateJwtToken(string Username,string Role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Username.ToString()),
                    new Claim(ClaimTypes.Role, Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenData = new tokenResponse
            {
                token = tokenHandler.WriteToken(token),
                issueTime = token.ValidFrom,
                expiryTime = token.ValidTo,
                expiresIn = (token.ValidTo - token.ValidFrom).TotalSeconds,
                role = Role
            };
            return tokenData;
        }
    }
}
