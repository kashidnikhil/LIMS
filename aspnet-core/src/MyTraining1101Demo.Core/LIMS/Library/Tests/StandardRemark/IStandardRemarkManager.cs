namespace MyTraining1101Demo.LIMS.Library.Tests.StandardRemark
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardReference.Dto;

    using System.Threading.Tasks;

    using System;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardRemark.Dto;

    public interface IStandardRemarkManager : IDomainService
    {
        Task<PagedResultDto<StandardRemarkDto>> GetPaginatedStandardRemarkListFromDB(StandardRemarkSearchDto input);
        Task<Guid> InsertOrUpdateStandardRemarkIntoDB(StandardRemarkInputDto input);

        Task<bool> DeleteStandardRemarkFromDB(Guid standardRemarkId);

        Task<StandardRemarkDto> GetStandardRemarkByIdFromDB(Guid standardRemarkId);
    }
}
