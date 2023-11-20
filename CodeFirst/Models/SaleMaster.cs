using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeFirst.Models
{
    public class SaleMaster
    {
        [Key]
        public int SaleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public string ProductType { get; set; }
        public virtual IList<SaleDetail> SaleDetails { get; set; }
    }
}