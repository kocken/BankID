using Newtonsoft.Json;

namespace BankID.Client.Models.Completed
{
    public class DeviceResponseDTO
    {
        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; }
    }
}
