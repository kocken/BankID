using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client.Models
{
    public class OrderRequestDTO
    {
        [JsonProperty("orderRef")]
        public string OrderRef { get; set; } // required
    }
}
