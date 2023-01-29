namespace MyTraining1101Demo.LIMS.Library.Tests.Unit
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Tests.Technique.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Unit.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class UnitManager : MyTraining1101DemoDomainServiceBase, IUnitManager
    {
        private readonly IRepository<Unit, Guid> _unitRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public UnitManager(
           IRepository<Unit, Guid> unitRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _unitRepository = unitRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<UnitDto>> GetPaginatedUnitFromDB(UnitSearchDto input)
        {
            try
            {
                var taxQuery = this._unitRepository.GetAll()
                    .Where(x => x.IsDeleted == false)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await taxQuery.CountAsync();
                var items = await taxQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<UnitDto>(
                totalCount,
                ObjectMapper.Map<List<UnitDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateUnitIntoDB(UnitInputDto input)
        {
            try
            {
                var mappedUnitItem = ObjectMapper.Map<Unit>(input);
                var unitId = await this._unitRepository.InsertOrUpdateAndGetIdAsync(mappedUnitItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return unitId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteUnitFromDB(Guid unitId)
        {
            try
            {
                var unitItem = await this._unitRepository.GetAsync(unitId);

                await this._unitRepository.DeleteAsync(unitItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<UnitDto> GetUnitByIdFromDB(Guid unitId)
        {
            try
            {
                var unitItem = await this._unitRepository.GetAsync(unitId);

                return ObjectMapper.Map<UnitDto>(unitItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
