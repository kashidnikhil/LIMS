using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster
{
    [Table("Customer")]
    public class Customer : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }

        public string Initials { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public string CommonDescription { get; set; }

        public string GSTNumber { get; set; }

        public string MobileNumber { get; set; }

        public string EmailId { get; set; }

        public string CommercialDescription { get; set; }

        [Required(ErrorMessage = "Enter discount")]
        [Precision(18, 2)]
        public decimal Discount { get; set; }

        public string Industry { get; set; }
        public string Website { get; set; }
        public string SpecialDescription { get; set; }

        public bool isSEZ { get; set; }

        public virtual Guid? ReferenceId { get; set; }
        public virtual Customer Reference { get; set; }

    }
}
