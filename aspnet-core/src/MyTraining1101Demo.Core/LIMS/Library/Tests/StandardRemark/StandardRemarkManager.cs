namespace MyTraining1101Demo.LIMS.Library.Tests.StandardRemark
{
    using Abp.Domain.Repositories;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
     using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardRemark.Dto;
    using Abp.Domain.Uow;
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Shared;

    public class StandardRemarkManager : MyTraining1101DemoDomainServiceBase, IStandardRemarkManager
    {
        private readonly IRepository<StandardRemark, Guid> _standardRemarkRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public StandardRemarkManager(
           IRepository<StandardRemark, Guid> standardRemarkRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _standardRemarkRepository = standardRemarkRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<StandardRemarkDto>> GetPaginatedStandardRemarkListFromDB(StandardRemarkSearchDto input)
        {
            try
            {
                var standardRemarkQuery = this._standardRemarkRepository.GetAll()
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await standardRemarkQuery.CountAsync();
                var items = await standardRemarkQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<StandardRemarkDto>(
                totalCount,
                ObjectMapper.Map<List<StandardRemarkDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<ResponseDto> InsertOrUpdateStandardRemarkIntoDB(StandardRemarkInputDto input)
        {
            try
            {
                Guid standardRemarkId = Guid.Empty;
                var standardRemarkItem = await this._standardRemarkRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == input.Name.ToLower().Trim());
                if (standardRemarkItem != null)
                {
                    if (input.Id != standardRemarkItem.Id) {
                        return new ResponseDto
                        {
                            Id = input.Id == Guid.Empty ? null : input.Id,
                            Name = standardRemarkItem.Name,
                            IsExistingDataAlreadyDeleted = standardRemarkItem.IsDeleted,
                            DataMatchFound = true,
                            RestoringItemId = standardRemarkItem.Id
                        };
                    }
                    else
                    {
                        standardRemarkItem.Name = input.Name;
                        standardRemarkItem.Description = input.Description;
                        standardRemarkId = await this._standardRemarkRepository.InsertOrUpdateAndGetIdAsync(standardRemarkItem);
                        return new ResponseDto
                        {
                            Id = standardRemarkId,
                            DataMatchFound = false
                        };
                    }
                   
                }
                else
                {
                    var mappedApplicationItem = ObjectMapper.Map<StandardRemark>(input);
                    standardRemarkId = await this._standardRemarkRepository.InsertOrUpdateAndGetIdAsync(mappedApplicationItem);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return new ResponseDto
                    {
                        Id = standardRemarkId,
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
        public async Task<bool> DeleteStandardRemarkFromDB(Guid standardRemarkId)
        {
            try
            {
                var standardRemarkItem = await this._standardRemarkRepository.GetAsync(standardRemarkId);

                await this._standardRemarkRepository.DeleteAsync(standardRemarkItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<StandardRemarkDto> GetStandardRemarkByIdFromDB(Guid standardRemarkId)
        {
            try
            {
                var standardRemarkItem = await this._standardRemarkRepository.GetAsync(standardRemarkId);

                return ObjectMapper.Map<StandardRemarkDto>(standardRemarkItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> RestoreStandardRemark(Guid standardRemarkId)
        {
            try
            {
                var standardRemarkItem = await this._standardRemarkRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == standardRemarkId);
                standardRemarkItem.IsDeleted = false;
                standardRemarkItem.DeleterUserId = null;
                standardRemarkItem.DeletionTime = null;
                await this._standardRemarkRepository.UpdateAsync(standardRemarkItem);

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
