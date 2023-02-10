﻿namespace MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.CustomerContactPersons
{
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerContactPersons;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICustomerContactPersonManager : IDomainService
    {
        Task<Guid> BulkInsertOrUpdateCustomerContactPersons(List<ContactPersonInputDto> customerContactPersonsInputList);

        Task<bool> BulkDeleteCustomerContactPersons(Guid customerId);
        Task<IList<ContactPersonDto>> GetContactPersonListFromDB(Guid customerId);
    }
}