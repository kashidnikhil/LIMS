namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerAddresses;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerContactPersons;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerMasters;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerPOs;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerMasters;
    using Stripe;
    using System;
    using System.Threading.Tasks;

    public class CustomerMasterAppService : MyTraining1101DemoAppServiceBase,ICustomerMasterAppService
    {
        private readonly ICustomerMasterManager _customerMasterManager;
        private readonly ICustomerAddressManager _customerAddressManager;
        private readonly ICustomerPOManager _customerPOManager;
        private readonly ICustomerContactPersonManager _customerContactPersonManager;


        public CustomerMasterAppService(
            ICustomerMasterManager customerMasterManager,
            ICustomerAddressManager customerAddressManager,
            ICustomerPOManager customerPOManager,
            ICustomerContactPersonManager customerContactPersonManager
         )
        {
            _customerMasterManager = customerMasterManager;
            _customerAddressManager = customerAddressManager;
            _customerPOManager = customerPOManager;
            _customerContactPersonManager = customerContactPersonManager;
        }


        public async Task<PagedResultDto<CustomerMasterDto>> GetCustomers(CustomerMasterSearchDto input)
        {
            try
            {
                var result = await this._customerMasterManager.GetPaginatedCustomerListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<Guid> InsertOrUpdateCustomer(CustomerMasterInputDto input)
        {
            try
            {
                var insertedOrUpdatedCustomerId = await this._customerMasterManager.InsertOrUpdateCustomerIntoDB(input);

                if (insertedOrUpdatedCustomerId != Guid.Empty) {
                    if (input.CustomerAddresses != null && input.CustomerAddresses.Count > 0) {
                        input.CustomerAddresses.ForEach(customerAddress => {
                            customerAddress.CustomerId = insertedOrUpdatedCustomerId;
                        });
                        await this._customerAddressManager.BulkInsertOrUpdateCustomerAddresses(input.CustomerAddresses);
                    }
                    if (input.CustomerContactPersons != null && input.CustomerContactPersons.Count > 0)
                    {
                        input.CustomerContactPersons.ForEach(customerContactPerson => {
                            customerContactPerson.CustomerId = insertedOrUpdatedCustomerId;
                        });
                        await this._customerContactPersonManager.BulkInsertOrUpdateCustomerContactPersons(input.CustomerContactPersons);
                    }
                    if (input.CustomerPOs != null && input.CustomerPOs.Count > 0) {
                        input.CustomerPOs.ForEach(customerPO => {
                            customerPO.CustomerId = insertedOrUpdatedCustomerId;
                        });
                        await this._customerPOManager.BulkInsertOrUpdateCustomerPOs(input.CustomerPOs);
                    }
                    
                }
                return insertedOrUpdatedCustomerId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteCustomerData(Guid customerId)
        {
            try
            {
                var isCustomerAddressDeleted = await this._customerAddressManager.BulkDeleteCustomerAddresses(customerId);
                
                var isCustomerPOsDeleted = await this._customerPOManager.BulkDeleteCustomerPOs(customerId);
                
                var isCustomerContactPersonsDeleted = await this._customerContactPersonManager.BulkDeleteCustomerContactPersons(customerId);
                
                var isCustomerDataDeleted = await this._customerMasterManager.DeleteCustomerFromDB(customerId);
                
                return isCustomerDataDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<CustomerMasterDto> GetCustomerById(Guid customerId)
        {
            try
            {
                var customerItem = await this._customerMasterManager.GetCustomerByIdFromDB(customerId);

                if (customerItem.Id != Guid.Empty) {
                    customerItem.CustomerAddresses = await this._customerAddressManager.GetCustomerAddressListFromDB(customerId);
                    customerItem.CustomerContactPersons = await this._customerContactPersonManager.GetContactPersonListFromDB(customerId);
                    customerItem.CustomerPOs = await this._customerPOManager.GetCustomerPOListFromDB(customerId);
                }
                
                return customerItem;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
