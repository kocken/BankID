using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankID.WebDemo.Models
{
    public class PersonalNumberModel
    {
        [Required]
        [MinLength(12, ErrorMessage = "The personal number must be 12 characters long.")]
        [MaxLength(12, ErrorMessage = "The personal number must be 12 characters long.")]
        public string PersonalNumber { get; set; }
    }
}