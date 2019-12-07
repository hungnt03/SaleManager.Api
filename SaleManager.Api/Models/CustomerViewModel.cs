using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models
{
    public class CustomerViewModel
    {
        [Display(Name = "Customer Id")]
        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string Name { get; set; }

        [StringLength(250)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Contact")]
        [StringLength(50)]
        public string Contact { get; set; }
    }
}
