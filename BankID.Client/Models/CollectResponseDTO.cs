using BankID.Client.Models.Completed;
using Newtonsoft.Json;

namespace BankID.Client.Models
{
    public class CollectResponseDTO
    {
        [JsonProperty("orderRef")]
        public string OrderRef { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("hintCode")]
        public string HintCode { get; set; }

        [JsonProperty("completionData")]
        public CompletionResponseDTO CompletionData { get; set; }

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
