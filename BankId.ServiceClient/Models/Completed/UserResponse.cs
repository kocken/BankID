using System.Text.Json.Serialization;

namespace BankId.ServiceClient.Models.Completed
{
    public class UserResponse
    {
        [JsonPropertyName("personalNumber")]
        public string PersonalNumber { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("surname")]
        public string SurName { get; set; }
    }
}
