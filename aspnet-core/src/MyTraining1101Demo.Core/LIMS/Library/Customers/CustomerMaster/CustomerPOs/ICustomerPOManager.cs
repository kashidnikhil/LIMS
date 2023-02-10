namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerPOs
{
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerPOs;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;

    public interface ICustomerPOManager : IDomainService
    {
        Task<Guid> BulkInsertOrUpdateCustomerPOs(List<CustomerPOInputDto> customerPOInputList);

        Task<bool> BulkDeleteCustomerPOs(Guid customerId);

        Task<IList<CustomerPODto>> GetCustomerPOListFromDB(Guid customerId);

    }
}
