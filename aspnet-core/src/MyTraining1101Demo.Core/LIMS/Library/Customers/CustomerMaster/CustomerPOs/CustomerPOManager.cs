namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerPOs
{
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerPOs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CustomerPOManager : MyTraining1101DemoDomainServiceBase, ICustomerPOManager
    {
        private readonly IRepository<CustomerPO, Guid> _customerPORepository;
        private readonly IConfigurationRoot _appConfiguration;


        public CustomerPOManager(
           IRepository<CustomerPO, Guid> customerPORepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _customerPORepository = customerPORepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<Guid> BulkInsertOrUpdateCustomerPOs(List<CustomerPOInputDto> customerPOInputList) {
            try
            {
                Guid customerPOId = Guid.Empty;
                for (int i = 0;i< customerPOInputList.Count; i++) {
                    customerPOId = (Guid)customerPOInputList[i].CustomerId;
                    await this.InsertOrUpdateCustomerPOIntoDB(customerPOInputList[i]);
                }
                return customerPOId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [UnitOfWork]
        private async Task InsertOrUpdateCustomerPOIntoDB(CustomerPOInputDto input)
        {
            try
            {
                var mappedCustomerPOItem = ObjectMapper.Map<CustomerPO>(input);
                var customerPOId = await this._customerPORepository.InsertOrUpdateAndGetIdAsync(mappedCustomerPOItem);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<bool> BulkDeleteCustomerPOs(Guid customerId)
        {

            try
            {
                var customerPOs = await this.GetCustomerPOListFromDB(customerId);

                if (customerPOs.Count > 0)
                {
                    for (int i = 0; i < customerPOs.Count; i++)
                    {
                        await this.DeleteCustomerPOFromDB(customerPOs[i].Id);
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
        private async Task<bool> DeleteCustomerPOFromDB(Guid customerPOId)
        {
            try
            {
                var customerPOItem = await this._customerPORepository.GetAsync(customerPOId);

                await this._customerPORepository.DeleteAsync(customerPOItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<IList<CustomerPODto>> GetCustomerPOListFromDB(Guid customerId)
        {
            try
            {
                var customerAddressQuery = this._customerPORepository.GetAll()
                    .Where(x => !x.IsDeleted && x.CustomerId == customerId);

                return new List<CustomerPODto>(ObjectMapper.Map<List<CustomerPODto>>(customerAddressQuery));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }
    }
}
