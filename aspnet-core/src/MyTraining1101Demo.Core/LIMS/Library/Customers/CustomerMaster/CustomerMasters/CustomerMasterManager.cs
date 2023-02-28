namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerMasters
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerMasters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class CustomerMasterManager : MyTraining1101DemoDomainServiceBase, ICustomerMasterManager
    {
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public CustomerMasterManager(
           IRepository<Customer, Guid> customerRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _customerRepository = customerRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<CustomerMasterDto>> GetPaginatedCustomerListFromDB(CustomerMasterSearchDto input)
        {
            try
            {
                var containerQuery = this._customerRepository.GetAll()
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await containerQuery.CountAsync();
                var items = await containerQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<CustomerMasterDto>(
                totalCount,
                ObjectMapper.Map<List<CustomerMasterDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateCustomerIntoDB(CustomerMasterInputDto input)
        {
            try
            {
                var mappedCustomerItem = ObjectMapper.Map<Customer>(input);
                var customerId = await this._customerRepository.InsertOrUpdateAndGetIdAsync(mappedCustomerItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return customerId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteCustomerFromDB(Guid customerId)
        {
            try
            {
                var customerItem = await this._customerRepository.GetAsync(customerId);

                await this._customerRepository.DeleteAsync(customerItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<CustomerMasterDto> GetCustomerByIdFromDB(Guid customerId)
        {
            try
            {
                var customerItem = await this._customerRepository.GetAsync(customerId);

                return ObjectMapper.Map<CustomerMasterDto>(customerItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }


    }
}
