using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models.Product
{
    public class ProductConditionModel
    {
        public string NameOrBarcode { set; get; }
        public int Category { set; get; }
        public int Supplier { set; get; }
    }
}
