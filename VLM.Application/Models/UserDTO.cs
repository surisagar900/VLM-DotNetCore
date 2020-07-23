using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLM.Core.Models
{
    public class UserDTO
    {
        [Required]
        [StringLength(25, ErrorMessage = "Min:6, Max:25", MinimumLength = 6)]
        [RegularExpression(@"^[a-z0-9]*$")]
        public string UserName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Min:1, Max:30", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Min:1, Max:30", MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9._]+@[a-z0-9.-]+.[a-z]{2,}$")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "should be 10 digit long")]
        [DataType(DataType.PhoneNumber)]
        public long Phone { get; set; }
    }
}
