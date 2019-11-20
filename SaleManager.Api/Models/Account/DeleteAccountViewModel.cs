using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Models.Account
{
    public class DeleteAccountViewModel
    {
        [Required]
        public string Id { set; get; }
    }
}
