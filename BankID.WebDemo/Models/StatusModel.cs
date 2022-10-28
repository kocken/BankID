using BankId.ServiceClient.Models;
using System.Text.Json.Serialization;

namespace BankId.WebDemo.Models
{
    public class StatusModel
    {
        [JsonPropertyName("collectResponse")]
        public CollectResponse CollectResponse { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("userMessage")]
        public string UserMessage { get; set; }

        public StatusModel(CollectResponse collectResponse, string status, string userMessage)
        {
            this.CollectResponse = collectResponse;
            this.Status = status;
            this.UserMessage = userMessage;
        }
    }
}