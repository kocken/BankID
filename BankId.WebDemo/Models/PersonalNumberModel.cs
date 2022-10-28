using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankId.WebDemo.Models
{
    public class PersonalNumberModel
    {
        // TODO: add personal number pattern validation
        [Required(ErrorMessage = "Personal number must be assigned.")]
        [MinLength(12, ErrorMessage = "Personal number must be 12 numbers long.")]
        [MaxLength(12, ErrorMessage = "Personal number must be 12 numbers long.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Personal number may only contain numbers.")]
        [JsonPropertyName("personalNumber")]
        public string PersonalNumber { get; set; }

        public PersonalNumberModel()
        {
            PersonalNumber = string.Empty;
        }
    }
}