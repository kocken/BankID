using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models
{
    public class AuthorizeResponseDTO
    {
        [JsonProperty("autoStartToken")]
        public string AutoStartToken { get; set; }

        [JsonProperty("orderRef")]
        public string OrderRef { get; set; }
    }
}
