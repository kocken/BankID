using System.Text.Json.Serialization;

namespace BankId.ServiceClient.Models
{
    public class OrderRequest
    {
        [JsonPropertyName("orderRef")]
        public string OrderReference { get; set; } // required
    }
}
