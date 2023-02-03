namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplications
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.SubApplications.Dto;
    using System;
    using System.Threading.Tasks;

    public interface ISubApplicationManager : IDomainService
    {
        Task<PagedResultDto<SubApplicationDto>> GetPaginatedSubApplicationsListFromDB(SubApplicationSearchDto input);

        Task<Guid> InsertOrUpdateSubApplicationIntoDB(SubApplicationInputDto input);

        Task<bool> DeleteSubApplicationFromDB(Guid subApplicationId);

        Task<SubApplicationDto> GetSubApplicationByIdFromDB(Guid subApplicationId);
    }
}
