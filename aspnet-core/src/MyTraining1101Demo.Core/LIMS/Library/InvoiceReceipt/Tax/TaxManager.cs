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
    using MyTraining1101Demo.LIMS.Shared;
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
        public async Task<ResponseDto> InsertOrUpdateTaxIntoDB(TaxInputDto input)
        {
            try
            {
                Guid taxId = Guid.Empty;
                var taxItem = await this._taxRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == input.Name.ToLower().Trim());
                if (taxItem != null && input.Id != taxItem.Id)
                {
                    return new ResponseDto
                    {
                        Id = input.Id == Guid.Empty ? null : input.Id,
                        Name = taxItem.Name,
                        IsExistingDataAlreadyDeleted = taxItem.IsDeleted,
                        DataMatchFound = true,
                        RestoringItemId = taxItem.Id
                    };
                }
                else
                {
                    var mappedTaxItem = ObjectMapper.Map<Tax>(input);
                    taxId = await this._taxRepository.InsertOrUpdateAndGetIdAsync(mappedTaxItem);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return new ResponseDto
                    {
                        Id = taxId,
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

        public async Task<bool> RestoreTax(Guid taxId)
        {
            try
            {
                var taxItem = await this._taxRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == taxId);
                taxItem.IsDeleted = false;
                taxItem.DeleterUserId = null;
                taxItem.DeletionTime = null;
                await this._taxRepository.UpdateAsync(taxItem);

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
