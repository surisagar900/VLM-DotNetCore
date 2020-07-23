using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLM.Core.Helpers
{
    public class tokenResponse
    {
        public string token { get; set; }
        public DateTime expiryTime { get; set; }
        public DateTime issueTime { get; set; }
        public double expiresIn { get; set; }
        public string role { get; set; }
    }
}

