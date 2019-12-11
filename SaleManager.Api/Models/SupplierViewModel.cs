using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models
{
    public class SupplierViewModel
    {
        [Required]
        [Display(Name = "Category Id")]
        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [StringLength(250)]
        public string Contact1 { get; set; }
        [StringLength(250)]
        public string Contact2 { get; set; }
    }
}
