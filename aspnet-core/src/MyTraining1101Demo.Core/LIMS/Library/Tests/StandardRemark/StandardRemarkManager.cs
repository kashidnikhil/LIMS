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
        public async Task<Guid> InsertOrUpdateStandardRemarkIntoDB(StandardRemarkInputDto input)
        {
            try
            {
                var mappedStandardRemarkItem = ObjectMapper.Map<StandardRemark>(input);
                var standardRemarkId = await this._standardRemarkRepository.InsertOrUpdateAndGetIdAsync(mappedStandardRemarkItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return standardRemarkId;
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

    }
}
