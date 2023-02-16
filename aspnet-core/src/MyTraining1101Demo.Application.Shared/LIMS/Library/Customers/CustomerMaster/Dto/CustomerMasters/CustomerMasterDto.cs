namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerMasters
{
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerContactPersons;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerPOs;
    using System;
    using System.Collections.Generic;

    public class CustomerMasterDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Initials { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public string CommonDescription { get; set; }

        public string GSTNumber { get; set; }
        
        public string MobileNumber { get; set; }

        public string EmailId { get; set; }

        public string CommercialDescription { get; set; }

        public decimal Discount { get; set; }

        public string Industry { get; set; }
        public string Website { get; set; }
        public string SpecialDescription { get; set; }

        public bool isSEZ { get; set; }

        public virtual Guid? ReferenceId { get; set; }

        public IList<CustomerAddressDto> CustomerAddresses { get; set; }

        public IList<CustomerPODto> CustomerPOs { get; set; }

        public IList<ContactPersonDto> CustomerContactPersons { get; set; }
    }
}
