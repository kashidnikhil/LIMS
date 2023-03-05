namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface ITaxAppService
    {
        Task<PagedResultDto<TaxDto>> GetTaxes(TaxSearchDto input);
        Task<ResponseDto> InsertOrUpdateTax(TaxInputDto input);

        Task<bool> DeleteTax(Guid taxId);

        Task<bool> RestoreTax(Guid taxId);

        Task<TaxDto> GetTaxById(Guid taxId);
    }
}
