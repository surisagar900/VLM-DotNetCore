using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLM.Core.Entities
{
    public class Movies
    {
        [Key]
        public int MoviesId { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Min:1, Max:60", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9:""'\s-]*$")]
        public string Title { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Min:1, Max:30", MinimumLength = 1)]
        [RegularExpression(@"^[A-Za-z]*$")]
        public string Director { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Min:1, Max:30", MinimumLength = 1)]
        [RegularExpression(@"^[A-Za-z]*$")]
        public string Language { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Min:1, Max:30", MinimumLength = 1)]
        [RegularExpression(@"^[A-Za-z]*$")]
        public string Genre { get; set; }

        [Required]
        [Display(Name = "Released Year")]
        public int ReleaseYear { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Min:10, Max:500", MinimumLength = 10)]
        [RegularExpression(@"^[a-z0-9A-Z,./:""'\s-?!@$%*&()]*$")]
        public string Description { get; set; }

        [Display(Name = "Return Days")]
        [Range(1, 30, ErrorMessage = "Return days should be in between 1 and 30")]
        public int ReturnDays { get; set; }

        [Display(Name = "Fine Amount")]
        [Range(1, 1000, ErrorMessage = "Amount for fine should be in between 1 and 1000")]
        public int Fine { get; set; }
    }
}
