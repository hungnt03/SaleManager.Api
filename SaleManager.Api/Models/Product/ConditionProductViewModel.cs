using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models.Product
{
    public class ConditionProductViewModel
    {
        public string NameOrBarcode { set; get; }
        public int Category { set; get; }
        public int Supplier { set; get; }

        public bool IsEmpty()
        {
            return Category == -1 && Supplier == -1 && string.IsNullOrEmpty(NameOrBarcode);
        }
    }
}
