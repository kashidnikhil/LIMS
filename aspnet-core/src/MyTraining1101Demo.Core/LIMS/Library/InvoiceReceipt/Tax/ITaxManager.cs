namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax.Dto;
    using System;
    using System.Threading.Tasks;

    public interface ITaxManager : IDomainService
    {
        Task<PagedResultDto<TaxDto>> GetPaginatedTaxListFromDB(TaxSearchDto input);
        Task<Guid> InsertOrUpdateTaxIntoDB(TaxInputDto input);

        Task<bool> DeleteTaxFromDB(Guid taxId);

        Task<TaxDto> GetTaxByIdFromDB(Guid taxId);
    }
}
