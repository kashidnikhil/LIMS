namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface ITaxManager : IDomainService
    {
        Task<PagedResultDto<TaxDto>> GetPaginatedTaxListFromDB(TaxSearchDto input);
        Task<ResponseDto> InsertOrUpdateTaxIntoDB(TaxInputDto input);

        Task<bool> DeleteTaxFromDB(Guid taxId);

        Task<bool> RestoreTax(Guid taxId);

        Task<TaxDto> GetTaxByIdFromDB(Guid taxId);
    }
}
