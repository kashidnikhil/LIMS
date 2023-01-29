using Abp.Application.Services.Dto;
using MyTraining1101Demo.Dto;
using MyTraining1101Demo.PhoneBook.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1101Demo.PhoneBook
{
    public interface IPersonAppService 
    {
        ListResultDto<PersonListDto> GetPeople(GetPeopleInput input);

        Task CreatePerson(CreatePersonInput input);
        Task DeletePerson(EntityDto input);

        Task DeletePhone(EntityDto<long> input);
        Task<PhoneInPersonListDto> AddPhone(AddPhoneInput input);

        Task<GetPersonForEditOutput> GetPersonForEdit(GetPersonForEditInput input);

        Task EditPerson(EditPersonInput input);

    }
}
