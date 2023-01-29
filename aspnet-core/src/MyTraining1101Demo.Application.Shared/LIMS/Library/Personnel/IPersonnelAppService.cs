namespace MyTraining1101Demo.LIMS.Library.Personnel
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Personnel.Dto;
    using System;
    using System.Threading.Tasks;

    public interface IPersonnelAppService
    {
        Task<PagedResultDto<PersonnelDto>> GetPersonnels(PersonnelSearchDto input);
        Task<Guid> InsertOrUpdatePersonnel(PersonnelInputDto input);

        Task<bool> DeletePersonnel(Guid personnelId);

        Task<PersonnelDto> GetPersonnelById(Guid personnelId);
    }
}
