using BankID.Client.Models;
using Newtonsoft.Json;

namespace BankID.WebDemo.Models
{
    public class StatusModel
    {
        [JsonProperty("collectResponse")]
        public CollectResponseDTO CollectResponse { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("userMessage")]
        public string UserMessage { get; set; }

        public StatusModel(CollectResponseDTO collectResponse, string status, string userMessage)
        {
            this.CollectResponse = collectResponse;
            this.Status = status;
            this.UserMessage = userMessage;
        }
    }
}