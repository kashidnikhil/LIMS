namespace MyTraining1101Demo.LIMS.Library.Tests.StandardRemark
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardRemark.Dto;
    using System;
    using System.Threading.Tasks;

    public interface IStandardRemarkAppService
    {
        Task<PagedResultDto<StandardRemarkDto>> GetStandardRemarks(StandardRemarkSearchDto input);
        Task<Guid> InsertOrUpdateStandardRemark(StandardRemarkInputDto input);

        Task<bool> DeleteStandardRemark(Guid standardRemarkId);

        Task<StandardRemarkDto> GetStandardRemarkById(Guid standardRemarkId);
    }
}
