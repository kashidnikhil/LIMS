using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyTraining1101Demo.Customer.Dto
{
    public class CreateCustomerInput
    {
        [Required]
        [MaxLength(CustomerConsts.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CustomerConsts.MaxAddressLength)]
        public string Address { get; set; }

        [EmailAddress]
        [MaxLength(CustomerConsts.MaxEmailIdLength)]
        public string EmailId { get; set; }

        
        public System.DateTime RegistrationDate { get; set; }


    }
}
