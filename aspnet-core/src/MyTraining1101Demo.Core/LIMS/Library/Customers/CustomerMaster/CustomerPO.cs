namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster
{
    using Abp.Domain.Entities.Auditing;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CustomerPO")]
    public class CustomerPO : FullAuditedEntity<Guid>
    {
        public string POCode { get; set; }
        public DateTime PODate { get; set; }
        public string Description { get; set; }

        public DateTime CloseDate { get; set; }

        [Precision(18,2)]
        public decimal Amount { get; set; }

        public virtual Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
