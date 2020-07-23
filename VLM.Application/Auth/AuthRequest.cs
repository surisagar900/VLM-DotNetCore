using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLM.Core.Helpers;

namespace VLM.Core.Auth
{
    public class AuthRequest
    {
        [Required]
        [Display(Name = "Email address")]
        [RegularExpression("^[a-z0-9]{6,25}$")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Min:8, Max:20", MinimumLength = 8)]
        [RegularExpression(@"^[a-zA-Z0-9@*#]*$", ErrorMessage = "only a-z, A-Z, 0-9, wild characters(@,*,#)")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = Roles.User;
    }
}
