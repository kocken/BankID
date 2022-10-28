using System.Text.Json.Serialization;

namespace BankId.ServiceClient.Models.Completed
{
    public class CompletionResponse
    {
        [JsonPropertyName("user")]
        public UserResponse User { get; set; }

        [JsonPropertyName("device")]
        public DeviceResponse Device { get; set; }

        [JsonPropertyName("cert")]
        public CertificateResponse Certificate { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("ocspResponse")]
        public string OcspResponse { get; set; }
    }
}
