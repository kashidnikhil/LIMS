namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerMasters
{
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerPOs;
    using System.Threading.Tasks;
    using System;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerMasters;
    using Abp.Application.Services.Dto;

    public interface ICustomerMasterManager : IDomainService
    {
        Task<PagedResultDto<CustomerMasterDto>> GetPaginatedCustomerListFromDB(CustomerMasterSearchDto input);

        Task<Guid> InsertOrUpdateCustomerIntoDB(CustomerMasterInputDto input);

        Task<bool> DeleteCustomerFromDB(Guid customerId);

        Task<CustomerMasterDto> GetCustomerByIdFromDB(Guid customerId);
    }
}
