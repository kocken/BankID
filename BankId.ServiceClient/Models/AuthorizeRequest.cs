using System.Text.Json.Serialization;

namespace BankId.ServiceClient.Models
{
    public class AuthorizeRequest
    {
        [JsonPropertyName("endUserIp")]
        public string EndUserIp { get; set; } // required

        [JsonPropertyName("personalNumber")]
        public string PersonalNumber { get; set; } // optional

        [JsonPropertyName("requirement")]
        public string Requirement { get; set; } // optional

        [JsonPropertyName("userVisibleData")]
        public string UserVisibleData { get; set; } // required for sign, optional for auth

        [JsonPropertyName("userVisibleDataFormat")]
        public string UserVisibleDataFormat { get; set; } // optional

        [JsonPropertyName("userNonVisibleData")]
        public string UserNonVisibleData { get; set; } // optional
    }
}
