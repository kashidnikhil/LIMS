namespace MyTraining1101Demo.LIMS.Library.Tests.Application
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using MyTraining1101Demo.LIMS.Shared;

    public interface IApplicationsAppService
    {
        Task<PagedResultDto<ApplicationsDto>> GetApplications(ApplicationsSearchDto input);
        Task<ResponseDto> InsertOrUpdateApplication(ApplicationInputDto input);

        Task<bool> DeleteApplication(Guid applicationId);

        Task<ApplicationsDto> GetApplicationById(Guid applicationId);

        Task<IList<ApplicationsDto>> GetApplicationList();

        Task<bool> RevokeApplication(Guid applicationId);
    }
}
