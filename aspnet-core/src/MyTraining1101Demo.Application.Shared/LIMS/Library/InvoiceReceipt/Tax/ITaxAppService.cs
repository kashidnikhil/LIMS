namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax.Dto;
    using System;
    using System.Threading.Tasks;

    public interface ITaxAppService
    {
        Task<PagedResultDto<TaxDto>> GetTaxes(TaxSearchDto input);
        Task<Guid> InsertOrUpdateTax(TaxInputDto input);

        Task<bool> DeleteTax(Guid taxId);

        Task<TaxDto> GetTaxById(Guid taxId);
    }
}
