using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using MyTraining1101Demo.Configuration;
namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerContactPersons
{
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerContactPersons;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateCustomerContactPersonIntoDB(ContactPersonInputDto input)
        {
            try
            {
                var mappedContactPersonItem = ObjectMapper.Map<CustomerContactPerson>(input);
                var contactPersonId = await this._contactPersonRepository.InsertOrUpdateAndGetIdAsync(mappedContactPersonItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return contactPersonId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteContactPersonFromDB(Guid contactPersonId)
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
