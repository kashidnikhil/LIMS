namespace MyTraining1101Demo.LIMS.Library.Customers.Source
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Customers.Source.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using MyTraining1101Demo.LIMS.Shared;

    public class SourceManager : MyTraining1101DemoDomainServiceBase, ISourceManager
    {
        private readonly IRepository<Source, Guid> _sourceRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public SourceManager(
           IRepository<Source, Guid> sourceRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _sourceRepository = sourceRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<SourceDto>> GetPaginatedSourceListFromDB(SourceSearchDto input)
        {
            try
            {
                var sourceQuery = this._sourceRepository.GetAll()
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await sourceQuery.CountAsync();
                var items = await sourceQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<SourceDto>(
                totalCount,
                ObjectMapper.Map<List<SourceDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<ResponseDto> InsertOrUpdateSourceIntoDB(SourceInputDto input)
        {
            try
            {
                Guid sourceId = Guid.Empty;
                var sourceItem = await this._sourceRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == input.Name.ToLower().Trim());
                if (sourceItem != null && input.Id != sourceItem.Id)
                {
                    //if incoming data matches the existing data and 
                    return new ResponseDto
                    {
                        Id = input.Id == Guid.Empty ? null : input.Id,
                        Name = sourceItem.Name,
                        IsExistingDataAlreadyDeleted = sourceItem.IsDeleted,
                        DataMatchFound = true,
                        RestoringItemId = sourceItem.Id
                    };
                }
                else
                {
                    var mappedSourceItem = ObjectMapper.Map<Source>(input);
                    sourceId = await this._sourceRepository.InsertOrUpdateAndGetIdAsync(mappedSourceItem);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return new ResponseDto
                    {
                        Id = sourceId,
                        DataMatchFound = false
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteSourceFromDB(Guid sourceId)
        {
            try
            {
                var sourceItem = await this._sourceRepository.GetAsync(sourceId);

                await this._sourceRepository.DeleteAsync(sourceItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<SourceDto> GetSourceByIdFromDB(Guid sourceId)
        {
            try
            {
                var sourceItem = await this._sourceRepository.GetAsync(sourceId);

                return ObjectMapper.Map<SourceDto>(sourceItem);

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
                var sourceItem = await this._sourceRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == sourceId);
                sourceItem.IsDeleted = false;
                sourceItem.DeleterUserId = null;
                sourceItem.DeletionTime = null;
                await this._sourceRepository.UpdateAsync(sourceItem);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }
    }
}
