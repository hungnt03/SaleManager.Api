using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models.Account
{
    public class RoleModelView
    {
        [Required]
        public string Id { set; get; }
        [Required]
        public string Role { set; get; }
    }
}
