﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PaulyMacs.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int NumberOfStars { get; set; }
        public string CustomerComments { get; set; }

        //Foreign Keys
        
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}