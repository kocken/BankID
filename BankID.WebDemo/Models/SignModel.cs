using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankID.WebDemo.Models
{
    public class SignModel
    {
        [Required]
        public string UserVisibleData { get; set; }
        public string UserNonVisibleData { get; set; }
        public string EndUserIp { get; set; }
        public string PersonalNumber { get; set; }
        public string Requirement { get; set; }
    }
}