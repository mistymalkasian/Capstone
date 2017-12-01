using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PaulyMacs.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool isOrderOpen { get; set; }
        public string OrderContents { get; set; }

        [ForeignKey("Id")]
        public virtual Customer Customer { get; set; }
    }
}