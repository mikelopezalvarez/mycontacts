using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyContacts.Models
{
    public class UserVM
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Rememberme { get; set; }
    }
}