namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerPOs
{
    using System;

    public class CustomerPOInputDto
    {
        public Guid? Id { get; set; }
        public string POCode { get; set; }
        public DateTime PODate { get; set; }
        public string Description { get; set; }

        public DateTime CloseDate { get; set; }

        public decimal Amount { get; set; }

        public Guid? CustomerId { get; set; }
    }
}
