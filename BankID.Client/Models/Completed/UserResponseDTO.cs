using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models.Completed
{
    public class UserResponseDTO
    {
        public string personalNumber { get; set; }
        public string name { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
    }
}
