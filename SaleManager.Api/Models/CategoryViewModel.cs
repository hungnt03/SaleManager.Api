using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models
{
    public class CategoryViewModel
    {
        [Display(Name = "Category Id")]
        [Required]
        public int Id { get; set; }

        [StringLength(250)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [StringLength(250)]
        [Display(Name = "Category Description")]
        public string Description { get; set; }
    }
}
