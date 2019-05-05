using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models.Completed
{
    public class DeviceResponseDTO
    {
        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; }
    }
}
