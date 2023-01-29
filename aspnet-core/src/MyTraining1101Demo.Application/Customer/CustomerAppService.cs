using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using MyTraining1101Demo.Customer.Dto;
using MyTraining1101Demo.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1101Demo.Customer
{
    public class CustomerAppService : MyTraining1101DemoAppServiceBase, ICustomerAppService
    {
        private readonly IRepository<customer> _customerRepository;
        public CustomerAppService(IRepository<customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public ListResultDto<CustomerListDto> GetPeople(GetCustomerInput input)
        {
            var customer = _customerRepository
             .GetAll()
             .WhereIf(
                 !input.Filter.IsNullOrEmpty(),
                 p => p.Name.Contains(input.Filter) ||
                      p.Address.Contains(input.Filter) ||
                      p.EmailId.Contains(input.Filter)
             )
             .OrderBy(p => p.Name)
             .ThenBy(p => p.Address)
             .ToList();

            return new ListResultDto<CustomerListDto>(ObjectMapper.Map<List<CustomerListDto>>(customer));

        }

        public async Task CreateCustomer(CreateCustomerInput input)
        {
            var customer = ObjectMapper.Map<customer>(input);
            await _customerRepository.InsertAsync(customer);
        }

    }
}
