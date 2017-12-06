using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PaulyMacs.Areas.Admin.Models
{
    public class Db : DbContext
    {
        public DbSet<Pages> Pages { get; set; }
    }
}