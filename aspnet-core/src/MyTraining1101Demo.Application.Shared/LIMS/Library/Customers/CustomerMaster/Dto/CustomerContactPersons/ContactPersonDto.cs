using System;

namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerContactPersons
{
    public class ContactPersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Designation { get; set; }
        public string Department { get; set; }
        public string DirectLine { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
