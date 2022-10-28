using System.Text.Json.Serialization;

namespace BankId.ServiceClient.Models.Completed
{
    public class DeviceResponse
    {
        [JsonPropertyName("ipAddress")]
        public string IpAddress { get; set; }
    }
}
