namespace MyTraining1101Demo.LIMS.Library.Customers.Source
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Customers.Source.Dto;
    using System;
    using System.Threading.Tasks;

    public interface ISourceManager : IDomainService
    {
        Task<PagedResultDto<SourceDto>> GetPaginatedSourceListFromDB(SourceSearchDto input);
        Task<Guid> InsertOrUpdateSourceIntoDB(SourceInputDto input);

        Task<bool> DeleteSourceFromDB(Guid sourceId);

        Task<SourceDto> GetSourceByIdFromDB(Guid sourceId);
    }
}
