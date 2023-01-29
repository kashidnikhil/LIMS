using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1101Demo.Customer
{
    [Table("PbCustomers")]
    public class customer : FullAuditedEntity
    {
        public const int MaxNameLength = 32;
        public const int MaxAddressLength = 255;
        public const int MaxEmailAddressLength = 255;

        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [MaxLength(MaxAddressLength)]
        public virtual string Address { get; set; }

        [MaxLength(MaxEmailAddressLength)]
        public virtual string EmailId { get; set; }

        public System.DateTime RegistrationDate { get; set; }
    }
}
