using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLM.Core.Helpers;

namespace VLM.Core.Auth
{
    public class AuthResponse
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public double ExpireInSec { get; set; }
        public string Role { get; set; }

        public AuthResponse(string username, tokenResponse tokenData)
        {
            Username = username;
            Token = tokenData.token;
            ExpireInSec = tokenData.expiresIn;
            Role = tokenData.role;
        }
    }
}
