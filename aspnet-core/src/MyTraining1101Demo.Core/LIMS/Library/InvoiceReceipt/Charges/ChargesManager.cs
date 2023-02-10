namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class ChargesManager : MyTraining1101DemoDomainServiceBase, IChargesManager
    {
        private readonly IRepository<Charges, Guid> _chargesRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public ChargesManager(
           IRepository<Charges, Guid> chargesRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _chargesRepository = chargesRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<ChargesDto>> GetPaginatedChargeListFromDB(ChargesSearchDto input)
        {
            try
            {
                var bankQuery = this._chargesRepository.GetAll()
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await bankQuery.CountAsync();
                var items = await bankQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<ChargesDto>(
                totalCount,
                ObjectMapper.Map<List<ChargesDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateChargeIntoDB(ChargesInputDto input)
        {
            try
            {
                var mappedChargeItem = ObjectMapper.Map<Charges>(input);
                var chargeId = await this._chargesRepository.InsertOrUpdateAndGetIdAsync(mappedChargeItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return chargeId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteChargeFromDB(Guid chargeId)
        {
            try
            {
                var chargeItem = await this._chargesRepository.GetAsync(chargeId);

                await this._chargesRepository.DeleteAsync(chargeItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<ChargesDto> GetChargeByIdFromDB(Guid chargeId)
        {
            try
            {
                var chargeItem = await this._chargesRepository.GetAsync(chargeId);

                return ObjectMapper.Map<ChargesDto>(chargeItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
