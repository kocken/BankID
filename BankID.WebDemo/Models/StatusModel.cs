using BankID.Client.Models;
using BankID.Client.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankID.WebDemo.Models
{
    public class StatusModel
    {
        public StatusModel(CollectResponseDTO collectResponse, string status, string userMessage)
        {
            this.CollectResponse = collectResponse;
            this.Status = status;
            this.UserMessage = userMessage;
        }

        public CollectResponseDTO CollectResponse { get; set; }
        public string Status { get; set; }
        public string UserMessage { get; set; }
    }
}