﻿namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster
{
    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CustomerAddress")]
    public class CustomerAddress : FullAuditedEntity<Guid>
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        public bool IsTemporaryDelete { get; set; }

        public virtual Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

    }
}
