using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models
{
    public class LoginViewModel
    {
        [RegularExpression("[0-9a-z]+")]
        [Required]
        [MaxLength(100)]
        public string Username { set; get; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
