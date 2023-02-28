namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerContactPersons
{
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerContactPersons;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto;

    public class CustomerContactPersonManager : MyTraining1101DemoDomainServiceBase, ICustomerContactPersonManager
    {
        private readonly IRepository<CustomerContactPerson, Guid> _contactPersonRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public CustomerContactPersonManager(
           IRepository<CustomerContactPerson, Guid> contactPersonRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _contactPersonRepository = contactPersonRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<Guid> BulkInsertOrUpdateCustomerContactPersons(List<ContactPersonInputDto> customerContactPersonsInputList)
        {
            try
            {
                Guid customerContactPersonId = Guid.Empty;
                var mappedContactPersons = ObjectMapper.Map<List<CustomerContactPerson>>(customerContactPersonsInputList);
                for (int i = 0; i < mappedContactPersons.Count; i++)
                {
                    customerContactPersonId = (Guid)mappedContactPersons[i].CustomerId;
                    await this.InsertOrUpdateCustomerContactPersonIntoDB(mappedContactPersons[i]);
                }
                return customerContactPersonId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [UnitOfWork]
        private async Task<Guid> InsertOrUpdateCustomerContactPersonIntoDB(CustomerContactPerson input)
        {
            try
            {
                var contactPersonId = await this._contactPersonRepository.InsertOrUpdateAndGetIdAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();
                return contactPersonId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }


        public async Task<bool> BulkDeleteCustomerContactPersons(Guid customerId)
        {

            try
            {
                var customerContactPersons = await this.GetContactPersonListFromDB(customerId);

                if (customerContactPersons.Count > 0)
                {
                    for (int i = 0; i < customerContactPersons.Count; i++)
                    {
                        await this.DeleteContactPersonFromDB(customerContactPersons[i].Id);
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
        private async Task<bool> DeleteContactPersonFromDB(Guid contactPersonId)
        {
            try
            {
                var contactPersonItem = await this._contactPersonRepository.GetAsync(contactPersonId);

                await this._contactPersonRepository.DeleteAsync(contactPersonItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<IList<ContactPersonDto>> GetContactPersonListFromDB(Guid customerId)
        {
            try
            {
                var customerAddressQuery = this._contactPersonRepository.GetAll()
                    .Where(x => !x.IsDeleted && x.CustomerId == customerId);

                return new List<ContactPersonDto>(ObjectMapper.Map<List<ContactPersonDto>>(customerAddressQuery));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }
    }
}
