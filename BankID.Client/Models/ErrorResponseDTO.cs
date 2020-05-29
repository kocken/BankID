using Newtonsoft.Json;

namespace BankID.Client.Models
{
    public class ErrorResponseDTO
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }
    }
}
