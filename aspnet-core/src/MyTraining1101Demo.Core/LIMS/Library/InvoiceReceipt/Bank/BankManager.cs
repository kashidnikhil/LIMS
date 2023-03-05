namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class BankManager : MyTraining1101DemoDomainServiceBase, IBankManager
    {
        private readonly IRepository<Bank, Guid> _bankRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public BankManager(
           IRepository<Bank, Guid> bankRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _bankRepository = bankRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<BankDto>> GetPaginatedBankListFromDB(BankSearchDto input)
        {
            try
            {
                var bankQuery = this._bankRepository.GetAll()
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await bankQuery.CountAsync();
                var items = await bankQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<BankDto>(
                totalCount,
                ObjectMapper.Map<List<BankDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<ResponseDto> InsertOrUpdateBankIntoDB(BankInputDto input)
        {
            try
            {
                Guid bankId = Guid.Empty;
                var bankItem = await this._bankRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == input.Name.ToLower().Trim());
                if (bankItem != null && input.Id != bankItem.Id)
                {
                    //if incoming data matches the existing data and 
                    return new ResponseDto
                    {
                        Id = input.Id == Guid.Empty ? null : input.Id,
                        Name = bankItem.Name,
                        IsExistingDataAlreadyDeleted = bankItem.IsDeleted,
                        DataMatchFound = true,
                        RestoringItemId = bankItem.Id
                    };
                }
                else
                {
                    var mappedBankItem = ObjectMapper.Map<Bank>(input);
                    bankId = await this._bankRepository.InsertOrUpdateAndGetIdAsync(mappedBankItem);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return new ResponseDto
                    {
                        Id = bankId,
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
        public async Task<bool> DeleteBankFromDB(Guid sourceId)
        {
            try
            {
                var bankItem = await this._bankRepository.GetAsync(sourceId);

                await this._bankRepository.DeleteAsync(bankItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<BankDto> GetBankByIdFromDB(Guid bankId)
        {
            try
            {
                var bankItem = await this._bankRepository.GetAsync(bankId);

                return ObjectMapper.Map<BankDto>(bankItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> RestoreBank(Guid bankId)
        {
            try
            {
                var bankItem = await this._bankRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == bankId);
                bankItem.IsDeleted = false;
                bankItem.DeleterUserId = null;
                bankItem.DeletionTime = null;
                await this._bankRepository.UpdateAsync(bankItem);

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
