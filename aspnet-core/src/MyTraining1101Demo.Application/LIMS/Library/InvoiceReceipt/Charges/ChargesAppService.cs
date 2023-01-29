using Abp.Application.Services.Dto;
using Microsoft.Extensions.Logging;
using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank.Dto;
using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank;
using System.Threading.Tasks;
using System;
using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges.Dto;

namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges
{
    public class ChargesAppService : MyTraining1101DemoAppServiceBase, IChargesAppService
    {
        private readonly IChargesManager _chargesManager;

        public ChargesAppService(
          IChargesManager chargesManager
         )
        {
            _chargesManager = chargesManager;
        }


        public async Task<PagedResultDto<ChargesDto>> GetCharges(ChargesSearchDto input)
        {
            try
            {
                var result = await this._chargesManager.GetPaginatedChargeListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<Guid> InsertOrUpdateCharges(ChargesInputDto input)
        {
            try
            {
                var insertedOrUpdatedChargeId = await this._chargesManager.InsertOrUpdateChargeIntoDB(input);

                return insertedOrUpdatedChargeId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteCharge(Guid chargeId)
        {
            try
            {
                var isChargeDeleted = await this._chargesManager.DeleteChargeFromDB(chargeId);
                return isChargeDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<ChargesDto> GetChargesById(Guid chargeId)
        {
            try
            {
                var response = await this._chargesManager.GetChargeByIdFromDB(chargeId);
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
