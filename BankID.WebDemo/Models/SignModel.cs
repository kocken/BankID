using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankId.WebDemo.Models
{
    public class SignModel
    {
        [Required]
        [JsonPropertyName("userVisibleData")]
        public string UserVisibleData { get; set; }

        [JsonPropertyName("userNonVisibleData")]
        public string UserNonVisibleData { get; set; }

        [JsonPropertyName("endUserIp")]
        public string EndUserIp { get; set; }

        [JsonPropertyName("personalNumber")]
        public string PersonalNumber { get; set; }

        [JsonPropertyName("requirement")]
        public string Requirement { get; set; }
    }
}