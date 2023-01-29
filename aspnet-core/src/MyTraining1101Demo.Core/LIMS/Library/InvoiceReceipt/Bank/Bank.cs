namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank
{
    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Bank")]
    public class Bank : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Bank Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
