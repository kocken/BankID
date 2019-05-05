using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models
{
    public class AuthorizeRequestDTO
    {
        [JsonProperty("endUserIp")]
        public string EndUserIp { get; set; } // required

        [JsonProperty("personalNumber")]
        public string PersonalNumber { get; set; } // optional

        [JsonProperty("Requirement")]
        public string Requirement { get; set; } // optional
    }
}
