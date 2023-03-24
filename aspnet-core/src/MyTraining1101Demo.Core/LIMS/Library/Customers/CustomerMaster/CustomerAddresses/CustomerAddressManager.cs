﻿namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerAddresses
{
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerContactPersons;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public async Task<Guid> BulkInsertOrUpdateCustomerAddresses(List<CustomerAddressInputDto> customerAddressInputList)
        {
            try
            {
                Guid customerId = Guid.Empty;
                var mappedCustomerAddresses = ObjectMapper.Map<List<CustomerAddress>>(customerAddressInputList);
                for (int i = 0; i < mappedCustomerAddresses.Count; i++)
                {
                    customerId = (Guid)mappedCustomerAddresses[i].CustomerId;
                    await this.InsertOrUpdateCustomerAddressIntoDB(mappedCustomerAddresses[i]);
                }
                return customerId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [UnitOfWork]
        private async Task InsertOrUpdateCustomerAddressIntoDB(CustomerAddress input)
        {
            try
            {
                var customerAddressId = await this._customerAddressRepository.InsertOrUpdateAndGetIdAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> BulkDeleteCustomerAddresses(Guid customerId) {

            try
            {
                var customerAddresses = await this.GetCustomerAddressListFromDB(customerId);

                if (customerAddresses.Count > 0) {
                    for (int i = 0; i < customerAddresses.Count; i++) {
                        await this.DeleteCustomerAddressFromDB(customerAddresses[i].Id);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [UnitOfWork]
        private async Task DeleteCustomerAddressFromDB(Guid customerAddressId)
        {
            try
            {
                var customerAddressItem = await this._customerAddressRepository.GetAsync(customerAddressId);

                await this._customerAddressRepository.DeleteAsync(customerAddressItem);

                await CurrentUnitOfWork.SaveChangesAsync();
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
