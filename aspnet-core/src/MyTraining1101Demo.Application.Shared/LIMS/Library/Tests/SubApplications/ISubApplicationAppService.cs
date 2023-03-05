namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplications
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.SubApplications.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface ISubApplicationAppService
    {
        Task<PagedResultDto<SubApplicationListDto>> GetSubApplications(SubApplicationSearchDto input);

        Task<ResponseDto> InsertOrUpdateSubApplication(SubApplicationInputDto input);

        Task<bool> DeleteSubApplication(Guid subApplicationId);

        Task<SubApplicationDto> GetSubApplicationById(Guid subApplicationId);

        Task<bool> RestoreSubApplication(Guid subApplicationId);
    }
}
