namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerAddresses
{
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CustomerAddressManager : MyTraining1101DemoDomainServiceBase, ICustomerAddressManager
    {
        private readonly IRepository<CustomerAddress, Guid> _customerAddressRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public CustomerAddressManager(
           IRepository<CustomerAddress, Guid> customerAddressRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _customerAddressRepository = customerAddressRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateCustomerAddressIntoDB(ApplicationInputDto input)
        {
            try
            {
                var mappedCustomerAddressItem = ObjectMapper.Map<CustomerAddress>(input);
                var customerAddressId = await this._customerAddressRepository.InsertOrUpdateAndGetIdAsync(mappedCustomerAddressItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return customerAddressId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteCustomerAddressFromDB(Guid customerAddressId)
        {
            try
            {
                var customerAddressItem = await this._customerAddressRepository.GetAsync(customerAddressId);

                await this._customerAddressRepository.DeleteAsync(customerAddressItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<IList<CustomerAddressDto>> GetCustomerAddressListFromDB(Guid customerId)
        {
            try
            {
                var customerAddressQuery = this._customerAddressRepository.GetAll()
                    .Where(x => !x.IsDeleted && x.CustomerId == customerId);

                return new List<CustomerAddressDto>(ObjectMapper.Map<List<CustomerAddressDto>>(customerAddressQuery));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }
    }
}
