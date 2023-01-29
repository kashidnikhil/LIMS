using System;
using System.Collections.Generic;
using System.Text;

namespace MyTraining1101Demo.Customer.Dto
{
    public class CustomerListDto
    {
        public int CustId { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string EmailId { get; set; }

        public System.DateTime RegistrationDate { get; set; }
    }
}
