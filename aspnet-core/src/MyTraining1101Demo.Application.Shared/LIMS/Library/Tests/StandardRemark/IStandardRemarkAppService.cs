namespace MyTraining1101Demo.LIMS.Library.Tests.StandardRemark
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardRemark.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface IStandardRemarkAppService
    {
        Task<PagedResultDto<StandardRemarkDto>> GetStandardRemarks(StandardRemarkSearchDto input);
        Task<ResponseDto> InsertOrUpdateStandardRemark(StandardRemarkInputDto input);

        Task<bool> DeleteStandardRemark(Guid standardRemarkId);

        Task<StandardRemarkDto> GetStandardRemarkById(Guid standardRemarkId);

        Task<bool> RestoreStandardRemark(Guid standardRemarkId);
    }
}
