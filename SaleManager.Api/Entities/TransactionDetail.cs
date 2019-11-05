using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Infrastructures.Entities
{
    [Table("TransactionDetail")]
    public class TransactionDetail:Auditable
    {
        [StringLength(12)]
        public string Barcode { get; set; }
        [Column(Order = 2)]
        public int TracsactionId { get; set; }
        public int SuplierId { get; set; }
        public int Quantity { get; set; }
        public bool IsPayment { get; set; }
        public decimal Amount { get; set; }
    }
}
