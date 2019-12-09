using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models.Product
{
    public class ProductSingleIdModel
    {
        [Required]
        [Display(Name = "Product Id")]
        [RegularExpression("([0-9]+)")]
        public string Barcode { set; get; }
    }
}
