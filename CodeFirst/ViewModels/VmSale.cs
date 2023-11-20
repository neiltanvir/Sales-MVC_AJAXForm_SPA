using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeFirst.ViewModels
{
    public class VmSale
    {
        public int SaleId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public string ProductType { get; set; }

        public string[] ProductName { get; set; }
        public decimal?[] Price { get; set; }
        public decimal?[] Qty { get; set; }
        public List<VmSaleDetail> SaleDetails { get; set; } = new List<VmSaleDetail>();

        public class VmSaleDetail
        {
            public int SaleDetailId { get; set; }
            public int? SaleId { get; set; }
            public string ProductName { get; set; }
            public decimal? Price { get; set; }
            public decimal? Qty { get; set; }
        }
    }
}