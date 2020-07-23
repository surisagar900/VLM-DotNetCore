using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLM.Core.Entities
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        
        [Required]
        [StringLength(30, ErrorMessage = "Min:1, Max:30", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Min:1, Max:30", MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9]{6,25}$", ErrorMessage = "Invalid username format")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Min:8, Max:20", MinimumLength = 8)]
        [RegularExpression(@"^[a-zA-Z0-9@*#]*$", ErrorMessage = "only a-z, A-Z, 0-9, wild characters(@,*,#)")]
        public string Password { get; set; }
    }
}
