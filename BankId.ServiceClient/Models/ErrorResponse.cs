using System.Text.Json.Serialization;

namespace BankId.ServiceClient.Models
{
    public class ErrorResponse
    {
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }
    }
}
