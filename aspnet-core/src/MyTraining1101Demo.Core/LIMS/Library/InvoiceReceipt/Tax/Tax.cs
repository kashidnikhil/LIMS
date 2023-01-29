using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax
{
    [Table("Tax")]
    public class Tax : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Tax Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter percentage")]
        [Precision(18, 2)]
        public decimal Percentage { get; set; }

    }
}
