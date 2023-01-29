using Abp.Application.Services.Dto;
using Abp.Domain.Services;
using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax.Dto;
using System.Threading.Tasks;
using System;
using MyTraining1101Demo.LIMS.Library.Personnel.Dto;

namespace MyTraining1101Demo.LIMS.Library.Personnel
{
    public interface IPersonnelManager : IDomainService
    {
        Task<PagedResultDto<PersonnelDto>> GetPaginatedPersonnelListFromDB(PersonnelSearchDto input);
        Task<Guid> InsertOrUpdatePersonnelIntoDB(PersonnelInputDto input);

        Task<bool> DeletePersonnelFromDB(Guid personnelId);

        Task<PersonnelDto> GetPersonnelByIdFromDB(Guid personnelId);
    }
}
