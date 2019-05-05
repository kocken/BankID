using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
