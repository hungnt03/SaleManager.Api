using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models.Product
{
    public class ProductModel
    {
        public string Barcode { set; get; }
        public string Name { set; get; }
        public Decimal Price { set; get; }
        public DateTime? ExpDate { set; get; }
        public int CategoryId { set; get; }
        public int SupplierId { set; get; }
        public int Quantity { set; get; }
        public bool Pin { set; get; }
        public bool Enable { set; get; }
        public string Unit { set; get; }
        public string Img { set; get; }
    }
}
