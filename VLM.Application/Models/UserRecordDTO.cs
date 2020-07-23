using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLM.Core.Entities;

namespace VLM.Core.Models
{
    public class UserRecordDTO
    {
        public int RecordsId { get; set; }

        public DateTime TakenDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public int Fine { get; set; } = 0;

        public string MovieTitle { get; set; }

        public bool isCleared { get; set; }
    }
}
