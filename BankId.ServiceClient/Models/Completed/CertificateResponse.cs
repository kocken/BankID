using System.Text.Json.Serialization;

namespace BankId.ServiceClient.Models.Completed
{
    public class CertificateResponse
    {
        [JsonPropertyName("notBefore")]
        public string NotBefore { get; set; }

        [JsonPropertyName("notAfter")]
        public string NotAfter { get; set; }
    }
}
