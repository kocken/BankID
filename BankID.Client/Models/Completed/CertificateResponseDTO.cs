using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models.Completed
{
    public class CertificateResponseDTO
    {
        public string notBefore { get; set; }
        public string notAfter { get; set; }
    }
}
