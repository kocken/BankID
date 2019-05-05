using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models
{
    public class AuthorizeResponseDTO
    {
        public string autoStartToken { get; set; }
        public string orderRef { get; set; }
    }
}
