namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplications
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.SubApplications.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface ISubApplicationManager : IDomainService
    {
        Task<PagedResultDto<SubApplicationListDto>> GetPaginatedSubApplicationsListFromDB(SubApplicationSearchDto input);

        Task<ResponseDto> InsertOrUpdateSubApplicationIntoDB(SubApplicationInputDto input);

        Task<bool> DeleteSubApplicationFromDB(Guid subApplicationId);

        Task<SubApplicationDto> GetSubApplicationByIdFromDB(Guid subApplicationId);

        Task<bool> RestoreSubApplication(Guid subApplicationId);
    }
}
