using Newtonsoft.Json;

namespace BankID.Client.Models
{
    public class OrderRequestDTO
    {
        [JsonProperty("orderRef")]
        public string OrderRef { get; set; } // required
    }
}
