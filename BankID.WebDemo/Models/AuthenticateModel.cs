using Newtonsoft.Json;

namespace BankID.WebDemo.Models
{
    public class AuthenticateModel
    {
        [JsonProperty("endUserIp")]
        public string EndUserIp { get; set; }

        [JsonProperty("personalNumber")]
        public string PersonalNumber { get; set; }

        [JsonProperty("requirement")]
        public string Requirement { get; set; }
    }
}