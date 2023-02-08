namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerAddresses
{
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerAddressManager : IDomainService
    {
        Task<IList<CustomerAddressDto>> GetCustomerAddressListFromDB(Guid customerId);

        Task<bool> DeleteCustomerAddressFromDB(Guid customerAddressId);

        Task<Guid> InsertOrUpdateCustomerAddressIntoDB(ApplicationInputDto input);
    }
}
