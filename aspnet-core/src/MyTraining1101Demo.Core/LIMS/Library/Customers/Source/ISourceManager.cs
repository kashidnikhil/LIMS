namespace MyTraining1101Demo.LIMS.Library.Customers.Source
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Customers.Source.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface ISourceManager : IDomainService
    {
        Task<PagedResultDto<SourceDto>> GetPaginatedSourceListFromDB(SourceSearchDto input);
        Task<ResponseDto> InsertOrUpdateSourceIntoDB(SourceInputDto input);

        Task<bool> DeleteSourceFromDB(Guid sourceId);

        Task<SourceDto> GetSourceByIdFromDB(Guid sourceId);

        Task<bool> RestoreSource(Guid sourceId);
    }
}
