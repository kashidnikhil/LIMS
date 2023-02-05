using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster
{
    [Table("CustomerAddress")]
    public class CustomerAddress : FullAuditedEntity<Guid>
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        public virtual Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

    }
}
