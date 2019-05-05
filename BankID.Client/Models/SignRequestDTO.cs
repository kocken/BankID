using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models
{
    public class SignRequestDTO : AuthorizeRequestDTO
    {
        [JsonProperty("userVisibleData")]
        public string UserVisibleData { get; set; } // required

        [JsonProperty("userNonVisibleData")]
        public string UserNonVisibleData { get; set; } // optional
    }
}
