using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models.Product
{
    public class UpdateProductViewModel
    {
        [StringLength(200)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }
        [Display(Name = "SupplierId")]
        public int SupplierId { get; set; }
        [Display(Name = "Pin")]
        public bool? Pin { get; set; }
        [Display(Name = "Enable")]
        public bool Enable { get; set; }
        [Display(Name = "ExpirationDate")]
        public DateTime? ExpirationDate { set; get; }
        [StringLength(50)]
        [Display(Name = "Unit")]
        public string Unit { set; get; }
        [Display(Name = "BarcodeImg")]
        public string BarcodeImg { set; get; }
    }
}
