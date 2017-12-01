using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PaulyMacs.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int NumberOfStars { get; set; }
        public string CustomerComments { get; set; }

        [ForeignKey("Id")]
        public virtual Customer Customer { get; set; }
    }
}