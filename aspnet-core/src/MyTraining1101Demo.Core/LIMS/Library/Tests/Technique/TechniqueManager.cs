﻿namespace MyTraining1101Demo.LIMS.Library.Tests.Technique
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class TechniqueManager : MyTraining1101DemoDomainServiceBase, ITechniqueManager
    {
        private readonly IRepository<Technique, Guid> _techniqueRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public TechniqueManager(
           IRepository<Technique, Guid> techniqueRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _techniqueRepository = techniqueRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<TechniqueDto>> GetPaginatedTechniqueFromDB(TechniqueSearchDto input)
        {
            try
            {
                var taxQuery = this._techniqueRepository.GetAll()
                    .Where(x => x.IsDeleted == false)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await taxQuery.CountAsync();
                var items = await taxQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<TechniqueDto>(
                totalCount,
                ObjectMapper.Map<List<TechniqueDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateTechniqueIntoDB(TechniqueInputDto input)
        {
            try
            {
                var mappedTechniqueItem = ObjectMapper.Map<Technique>(input);
                var techniqueId = await this._techniqueRepository.InsertOrUpdateAndGetIdAsync(mappedTechniqueItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return techniqueId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteTechniqueFromDB(Guid techniqueId)
        {
            try
            {
                var techniqueItem = await this._techniqueRepository.GetAsync(techniqueId);

                await this._techniqueRepository.DeleteAsync(techniqueItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<TechniqueDto> GetTechniqueByIdFromDB(Guid techniqueId)
        {
            try
            {
                var techniqueItem = await this._techniqueRepository.GetAsync(techniqueId);

                return ObjectMapper.Map<TechniqueDto>(techniqueItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
