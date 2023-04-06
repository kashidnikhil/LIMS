namespace MyTraining1101Demo.LIMS.Library.Tests.StandardRemark
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardReferences.Dto;

    using System.Threading.Tasks;

    using System;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardRemark.Dto;
    using MyTraining1101Demo.LIMS.Shared;

    public interface IStandardRemarkManager : IDomainService
    {
        Task<PagedResultDto<StandardRemarkDto>> GetPaginatedStandardRemarkListFromDB(StandardRemarkSearchDto input);
        Task<ResponseDto> InsertOrUpdateStandardRemarkIntoDB(StandardRemarkInputDto input);

        Task<bool> DeleteStandardRemarkFromDB(Guid standardRemarkId);

        Task<StandardRemarkDto> GetStandardRemarkByIdFromDB(Guid standardRemarkId);

        Task<bool> RestoreStandardRemark(Guid standardRemarkId);
    }
}
