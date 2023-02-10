namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class TaxManager : MyTraining1101DemoDomainServiceBase, ITaxManager
    {
        private readonly IRepository<Tax, Guid> _taxRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public TaxManager(
           IRepository<Tax, Guid> taxRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _taxRepository = taxRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<TaxDto>> GetPaginatedTaxListFromDB(TaxSearchDto input)
        {
            try
            {
                var taxQuery = this._taxRepository.GetAll()
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await taxQuery.CountAsync();
                var items = await taxQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<TaxDto>(
                totalCount,
                ObjectMapper.Map<List<TaxDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateTaxIntoDB(TaxInputDto input)
        {
            try
            {
                var mappedTaxItem = ObjectMapper.Map<Tax>(input);
                var taxId = await this._taxRepository.InsertOrUpdateAndGetIdAsync(mappedTaxItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return taxId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteTaxFromDB(Guid taxId)
        {
            try
            {
                var taxItem = await this._taxRepository.GetAsync(taxId);

                await this._taxRepository.DeleteAsync(taxItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<TaxDto> GetTaxByIdFromDB(Guid taxId)
        {
            try
            {
                var taxItem = await this._taxRepository.GetAsync(taxId);

                return ObjectMapper.Map<TaxDto>(taxItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
