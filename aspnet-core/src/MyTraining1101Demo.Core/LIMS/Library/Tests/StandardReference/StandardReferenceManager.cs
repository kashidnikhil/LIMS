namespace MyTraining1101Demo.LIMS.Library.Tests.StandardReference
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardReference.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class StandardReferenceManager: MyTraining1101DemoDomainServiceBase, IStandardReferenceManager
    {
        private readonly IRepository<StandardReference, Guid> _standardReferenceRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public StandardReferenceManager(
           IRepository<StandardReference, Guid> standardReferenceRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _standardReferenceRepository = standardReferenceRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<StandardReferenceDto>> GetPaginatedStandardReferenceListFromDB(StandardReferenceSearchDto input)
        {
            try
            {
                var taxQuery = this._standardReferenceRepository.GetAll()
                    .Where(x => x.IsDeleted == false)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await taxQuery.CountAsync();
                var items = await taxQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<StandardReferenceDto>(
                totalCount,
                ObjectMapper.Map<List<StandardReferenceDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateStandardReferenceIntoDB(StandardReferenceInputDto input)
        {
            try
            {
                var mappedStandardReferenceItem = ObjectMapper.Map<StandardReference>(input);
                var standardReferenceId = await this._standardReferenceRepository.InsertOrUpdateAndGetIdAsync(mappedStandardReferenceItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return standardReferenceId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteStandardReferenceFromDB(Guid standardReferenceId)
        {
            try
            {
                var standardReferenceItem = await this._standardReferenceRepository.GetAsync(standardReferenceId);

                await this._standardReferenceRepository.DeleteAsync(standardReferenceItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<StandardReferenceDto> GetStandardReferenceByIdFromDB(Guid standardReferenceId)
        {
            try
            {
                var standardReferenceItem = await this._standardReferenceRepository.GetAsync(standardReferenceId);

                return ObjectMapper.Map<StandardReferenceDto>(standardReferenceItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
