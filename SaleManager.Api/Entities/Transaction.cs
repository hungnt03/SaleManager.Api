using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Infrastructures.Entities
{
    [Table("Transaction")]
    public class Transaction:Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Type { get; set; }
        public int CustomerId { get; set; }
        public int SuplierId { get; set; }
        public bool IsPayment { get; set; }
        public decimal Amount { get; set; }
        public decimal Payment { get; set; }
        public decimal PayBack { get; set; }
        [StringLength(15)]
        public string BillNumber { get; set; }
    }
}
