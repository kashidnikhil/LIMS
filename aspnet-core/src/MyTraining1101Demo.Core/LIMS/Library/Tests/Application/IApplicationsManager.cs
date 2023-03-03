namespace MyTraining1101Demo.LIMS.Library.Tests.Application
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationsManager : IDomainService
    {
        Task<PagedResultDto<ApplicationsDto>> GetPaginatedApplicationsListFromDB(ApplicationsSearchDto input);
        Task<ResponseDto> InsertOrUpdateApplicationIntoDB(ApplicationInputDto input);

        Task<bool> DeleteApplicationFromDB(Guid applicationId);

        Task<ApplicationsDto> GetApplicationByIdFromDB(Guid applicationId);

        Task<IList<ApplicationsDto>> GetApplicationsListFromDB();

        Task<bool> RevokeApplication(Guid applicationId);

    }
}
