using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models
{
    public class AuthorizeRequestDTO
    {
        public string endUserIp { get; set; } // required
        public string personalNumber { get; set; } // optional
        public string Requirement { get; set; } // optional
    }
}
