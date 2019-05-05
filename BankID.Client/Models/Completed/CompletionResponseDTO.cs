using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models.Completed
{
    public class CompletionResponseDTO
    {
        [JsonProperty("user")]
        public UserResponseDTO User { get; set; }

        [JsonProperty("device")]
        public DeviceResponseDTO Device { get; set; }

        [JsonProperty("cert")]
        public CertificateResponseDTO Cert { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("ocspResponse")]
        public string OcspResponse { get; set; }
    }
}
