using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MVCEmail.Models
    {
        public class EmailForm
        {
            [Required, Display(Name = "Your name:")]
            public string FromName { get; set; }
            [Required, Display(Name = "Your email:"), EmailAddress]
            public string FromEmail { get; set; }
            [Required, Display(Name ="Your comments:")]
            public string Message { get; set; }
        }
    }