using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models
{
    public class UpdateAccountViewModel
    {
        [Required]
        public string Id { set; get; }

        [Display(Name = "FirstName")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Level")]
        public byte Level { get; set; }
        [Display(Name = "IsEnable")]
        public bool IsEnable { get; set; }
    }
}
