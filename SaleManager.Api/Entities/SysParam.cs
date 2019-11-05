using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaleManager.Api.Infrastructures.Entities
{
    [Table("SysParam")]
    public class SysParam:Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(250)]
        public string Key { get; set; }
        [StringLength(250)]
        public string Value { get; set; }
    }
}
