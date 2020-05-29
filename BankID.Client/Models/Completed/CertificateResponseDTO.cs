using Newtonsoft.Json;

namespace BankID.Client.Models.Completed
{
    public class CertificateResponseDTO
    {
        [JsonProperty("notBefore")]
        public string NotBefore { get; set; }

        [JsonProperty("notAfter")]
        public string NotAfter { get; set; }
    }
}
