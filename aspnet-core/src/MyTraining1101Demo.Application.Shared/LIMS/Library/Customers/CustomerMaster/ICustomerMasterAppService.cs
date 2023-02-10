namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerMasters;
    using System;
    using System.Threading.Tasks;

    public interface ICustomerMasterAppService : IDomainService
    {
        Task<PagedResultDto<CustomerMasterDto>> GetCustomers(CustomerMasterSearchDto input);

        Task<Guid> InsertOrUpdateCustomer(CustomerMasterInputDto input);

        Task<bool> DeleteCustomerData(Guid customerId);

        Task<CustomerMasterDto> GetCustomerById(Guid customerId);
    }
}
