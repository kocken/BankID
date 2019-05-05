using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models.Completed
{
    public class CompletionResponseDTO
    {
        public UserResponseDTO user { get; set; }
        public DeviceResponseDTO device { get; set; }
        public CertificateResponseDTO cert { get; set; }
        public string signature { get; set; }
        public string ocspResponse { get; set; }
    }
}
