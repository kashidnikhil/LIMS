﻿using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster
{
    [Table("ContactPerson")]
    public class CustomerContactPerson : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }

        public string Designation { get; set; }
        public string Department { get; set; }
        public string DirectLine { get; set; }
        public int MobileNumber { get; set; }
        public string EmailId { get; set; }
        public virtual Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
