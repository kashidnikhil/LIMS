namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplications
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.SubApplications.Dto;
    using System;
    using System.Threading.Tasks;

    public interface ISubApplicationAppService
    {
        Task<PagedResultDto<SubApplicationDto>> GetSubApplications(SubApplicationSearchDto input);

        Task<Guid> InsertOrUpdateSubApplication(SubApplicationInputDto input);

        Task<bool> DeleteSubApplication(Guid subApplicationId);

        Task<SubApplicationDto> GetSubApplicationById(Guid subApplicationId);
    }
}
