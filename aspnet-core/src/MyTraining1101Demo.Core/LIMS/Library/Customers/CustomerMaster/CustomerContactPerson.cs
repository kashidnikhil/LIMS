using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster
{
    [Table("ContactPerson")]
    public class CustomerContactPerson : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }

        public string Designation { get; set; }
        public string Department { get; set; }
        public string DirectLine { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }

        public bool IsTemporaryDelete { get; set; }
        public virtual Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
