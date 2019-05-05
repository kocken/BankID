using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models
{
    public class SignRequestDTO : AuthorizeRequestDTO
    {
        public string userVisibleData { get; set; } // required
        public string userNonVisibleData { get; set; } // optional
    }
}
