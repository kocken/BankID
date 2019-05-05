using BankID.Client.Models.Completed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return Status == "complete";
        }

        public bool IsFailed()
        {
            return Status == "failed";
        }

        public bool IsPending()
        {
            return Status == "pending";
        }
    }
}
