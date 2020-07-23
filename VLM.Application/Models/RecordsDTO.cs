using System;

namespace VLM.Core.Models
{
    public class RecordsDTO
    {
        public int RecordsId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public long PhoneNumber { get; set; }

        public string MoviesTitle { get; set; }

        public int MoviesReturnDays { get; set; }

        public int MoviesFine { get; set; }

        public DateTime TakenDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public int Fine { get; set; }

        public bool isCleared { get; set; }
    }
}
