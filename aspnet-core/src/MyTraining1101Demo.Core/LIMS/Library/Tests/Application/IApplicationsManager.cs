namespace MyTraining1101Demo.LIMS.Library.Tests.Application
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationsManager : IDomainService
    {
        Task<PagedResultDto<ApplicationsDto>> GetPaginatedApplicationsListFromDB(ApplicationsSearchDto input);
        Task<Guid> InsertOrUpdateApplicationIntoDB(ApplicationInputDto input);

        Task<bool> DeleteApplicationFromDB(Guid applicationId);

        Task<ApplicationsDto> GetApplicationByIdFromDB(Guid applicationId);

        Task<IList<ApplicationsDto>> GetApplicationsListFromDB();

    }
}
