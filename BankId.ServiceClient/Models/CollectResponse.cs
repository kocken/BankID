using BankId.ServiceClient.Models.Completed;
using System.Text.Json.Serialization;

namespace BankId.ServiceClient.Models
{
    public class CollectResponse
    {
        [JsonPropertyName("orderRef")]
        public string OrderReference { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("hintCode")]
        public string HintCode { get; set; }

        [JsonPropertyName("completionData")]
        public CompletionResponse CompletionData { get; set; }

        public bool IsComplete()
        {
            return Status?.ToLower() == "complete";
        }

        public bool IsFailed()
        {
            return Status?.ToLower() == "failed";
        }

        public bool IsPending()
        {
            return Status?.ToLower() == "pending";
        }
    }
}
