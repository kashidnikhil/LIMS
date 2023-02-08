namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto
{
    using System;
    public class CustomerAddressInputDto
    {
        public Guid? Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        public Guid? CustomerId { get; set; }
    }
}
