using Newtonsoft.Json;

namespace BankID.Client.Models.Completed
{
    public class UserResponseDTO
    {
        [JsonProperty("personalNumber")]
        public string PersonalNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("surname")]
        public string SurName { get; set; }
    }
}
