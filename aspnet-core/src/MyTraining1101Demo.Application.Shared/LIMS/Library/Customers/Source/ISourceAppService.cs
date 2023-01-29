namespace MyTraining1101Demo.LIMS.Library.Customers.Source
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Customers.Source.Dto;
    using System;
    using System.Threading.Tasks;

    public interface ISourceAppService
    {
        Task<PagedResultDto<SourceDto>> GetSources(SourceSearchDto input);
        Task<Guid> InsertOrUpdateSource(SourceInputDto input);

        Task<bool> DeleteSource(Guid sourceId);

        Task<SourceDto> GetSourceById(Guid sourceId);
    }
}
