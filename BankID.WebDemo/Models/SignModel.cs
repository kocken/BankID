using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BankID.WebDemo.Models
{
    public class SignModel
    {
        [Required]
        [JsonProperty("userVisibleData")]
        public string UserVisibleData { get; set; }

        [JsonProperty("userNonVisibleData")]
        public string UserNonVisibleData { get; set; }

        [JsonProperty("endUserIp")]
        public string EndUserIp { get; set; }

        [JsonProperty("personalNumber")]
        public string PersonalNumber { get; set; }

        [JsonProperty("requirement")]
        public string Requirement { get; set; }
    }
}