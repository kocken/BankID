using Newtonsoft.Json;

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
