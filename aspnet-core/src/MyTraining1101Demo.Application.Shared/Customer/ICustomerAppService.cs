using Abp.Application.Services.Dto;
using MyTraining1101Demo.Customer.Dto;
using MyTraining1101Demo.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1101Demo.Customer
{
    public interface ICustomerAppService
    {
        ListResultDto<CustomerListDto> GetPeople(GetCustomerInput input);

        Task CreateCustomer(CreateCustomerInput input);
    }
}
