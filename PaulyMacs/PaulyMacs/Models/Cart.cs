using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PaulyMacs.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public DateTime DateCreated { get; set; }

        public bool isCheckedOut { get; set; }

        //Foreign Key

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

    }
}