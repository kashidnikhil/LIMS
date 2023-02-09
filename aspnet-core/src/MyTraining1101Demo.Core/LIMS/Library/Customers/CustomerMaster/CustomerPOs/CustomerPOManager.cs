using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using MyTraining1101Demo.Configuration;
using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerContactPersons;
using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerContactPersons;
using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerPOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerPOs
{

    public class CustomerPOManager : MyTraining1101DemoDomainServiceBase //, ICustomerContactPersonManager
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

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateCustomerPOIntoDB(CustomerPOInputDto input)
        {
            try
            {
                var mappedCustomerPOItem = ObjectMapper.Map<CustomerPO>(input);
                var customerPOId = await this._customerPORepository.InsertOrUpdateAndGetIdAsync(mappedCustomerPOItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return customerPOId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteCustomerPOFromDB(Guid customerPOId)
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
