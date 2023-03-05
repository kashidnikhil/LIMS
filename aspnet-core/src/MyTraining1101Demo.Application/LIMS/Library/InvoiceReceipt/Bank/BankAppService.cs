namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public class BankAppService : MyTraining1101DemoAppServiceBase, IBankAppService
    {
        private readonly IBankManager _bankManager;

        public BankAppService(
          IBankManager bankManager
         )
        {
            _bankManager = bankManager;
        }


        public async Task<PagedResultDto<BankDto>> GetBanks(BankSearchDto input)
        {
            try
            {
                var result = await this._bankManager.GetPaginatedBankListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<ResponseDto> InsertOrUpdateBank(BankInputDto input)
        {
            try
            {
                var insertedOrUpdatedBankId = await this._bankManager.InsertOrUpdateBankIntoDB(input);

                return insertedOrUpdatedBankId;
            }

            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteBank(Guid bankId)
        {
            try
            {
                var isBankDeleted = await this._bankManager.DeleteBankFromDB(bankId);
                return isBankDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<BankDto> GetBankById(Guid bankId)
        {
            try
            {
                var response = await this._bankManager.GetBankByIdFromDB(bankId);
                return response;
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
                var response = await this._bankManager.RestoreBank(bankId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
