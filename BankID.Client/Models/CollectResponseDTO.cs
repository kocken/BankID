using BankID.Client.Models.Completed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models
{
    public class CollectResponseDTO
    {
        public string orderRef { get; set; }
        public string status { get; set; }
        public string hintCode { get; set; }
        public CompletionResponseDTO completionData { get; set; }

        public bool IsComplete()
        {
            return status == "complete";
        }

        public bool IsFailed()
        {
            return status == "failed";
        }

        public bool IsPending()
        {
            return status == "pending";
        }
    }
}
