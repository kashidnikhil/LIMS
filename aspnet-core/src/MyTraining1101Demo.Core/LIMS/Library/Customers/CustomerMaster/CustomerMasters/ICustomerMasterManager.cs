namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerMasters
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerMasters;
    using System;
    using System.Threading.Tasks;

    public interface ICustomerMasterManager : IDomainService
    {
        Task<PagedResultDto<CustomerMasterDto>> GetPaginatedCustomerListFromDB(CustomerMasterSearchDto input);

        Task<Guid> InsertOrUpdateCustomerIntoDB(CustomerMasterInputDto input);

        Task<bool> DeleteCustomerFromDB(Guid customerId);

        Task<CustomerMasterDto> GetCustomerByIdFromDB(Guid customerId);
    }
}
