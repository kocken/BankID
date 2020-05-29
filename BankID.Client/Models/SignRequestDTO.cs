using Newtonsoft.Json;

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
