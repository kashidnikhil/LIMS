namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public class TaxAppService : MyTraining1101DemoAppServiceBase, ITaxAppService
    {
        private readonly ITaxManager _taxManager;

        public TaxAppService(
          ITaxManager taxManager
         )
        {
            _taxManager = taxManager;
        }


        public async Task<PagedResultDto<TaxDto>> GetTaxes(TaxSearchDto input)
        {
            try
            {
                var result = await this._taxManager.GetPaginatedTaxListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<ResponseDto> InsertOrUpdateTax(TaxInputDto input)
        {
            try
            {
                var insertedOrUpdatedTaxId = await this._taxManager.InsertOrUpdateTaxIntoDB(input);

                return insertedOrUpdatedTaxId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteTax(Guid taxId)
        {
            try
            {
                var isTaxDeleted = await this._taxManager.DeleteTaxFromDB(taxId);
                return isTaxDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<TaxDto> GetTaxById(Guid taxId)
        {
            try
            {
                var response = await this._taxManager.GetTaxByIdFromDB(taxId);
                return response;
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
                var response = await this._taxManager.RestoreTax(taxId);
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
