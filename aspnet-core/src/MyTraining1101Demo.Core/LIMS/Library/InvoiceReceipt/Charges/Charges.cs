namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges
{
    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Charges")]
    public class Charges : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Charge Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
