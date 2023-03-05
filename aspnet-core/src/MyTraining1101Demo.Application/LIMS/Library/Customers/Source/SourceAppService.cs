namespace MyTraining1101Demo.LIMS.Library.Customers.Source
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Customers.Source.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public class SourceAppService : MyTraining1101DemoAppServiceBase, ISourceAppService
    {
        private readonly ISourceManager _sourceManager;


    public SourceAppService(
      ISourceManager sourceManager
     )
    {
        _sourceManager = sourceManager;
    }


    public async Task<PagedResultDto<SourceDto>> GetSources(SourceSearchDto input)
    {
        try
        {
            var result = await this._sourceManager.GetPaginatedSourceListFromDB(input);

            return result;
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message, ex);
            throw ex;
        }
    }
    public async Task<ResponseDto> InsertOrUpdateSource(SourceInputDto input)
    {
        try
        {
            var insertedOrUpdatedSourceId = await this._sourceManager.InsertOrUpdateSourceIntoDB(input);

            return insertedOrUpdatedSourceId;
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message, ex);
            throw ex;
        }
    }

    public async Task<bool> DeleteSource(Guid sourceId)
    {
        try
        {
            var isSourceDeleted = await this._sourceManager.DeleteSourceFromDB(sourceId);
            return isSourceDeleted;
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message, ex);
            throw ex;
        }

    }

        public async Task<SourceDto> GetSourceById(Guid sourceId)
        {
            try
            {
                var response = await this._sourceManager.GetSourceByIdFromDB(sourceId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> RestoreSource(Guid sourceId)
        {
            try
            {
                var response = await this._sourceManager.RestoreSource(sourceId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

    }
}
