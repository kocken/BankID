using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankID.WebDemo.Models
{
    public class AuthenticateModel
    {
        public string EndUserIp { get; set; }
        public string PersonalNumber { get; set; }
        public string Requirement { get; set; }
    }
}