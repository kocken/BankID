using System.Text.Json.Serialization;

namespace BankId.ServiceClient.Models
{
    public class AuthorizeResponse
    {
        [JsonPropertyName("orderRef")]
        public string OrderReference { get; set; }

        [JsonPropertyName("autoStartToken")]
        public string AutoStartToken { get; set; }

        [JsonPropertyName("qrStartToken")]
        public string QrStartToken { get; set; }

        [JsonPropertyName("qrStartSecret")]
        public string QrStartSecret { get; set; }
    }
}
