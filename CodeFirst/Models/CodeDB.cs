using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CodeFirst.Models
{
    public class CodeDB : DbContext
    {
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<SaleMaster> SaleMasters { get; set; }
    }
}