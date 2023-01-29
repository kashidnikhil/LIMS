namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges.Dto;
    using System;
    using System.Threading.Tasks;

    public interface IChargesAppService
    {
        Task<PagedResultDto<ChargesDto>> GetCharges(ChargesSearchDto input);
        Task<Guid> InsertOrUpdateCharges(ChargesInputDto input);

        Task<bool> DeleteCharge(Guid chargeId);

        Task<ChargesDto> GetChargesById(Guid chargeId);
    }
}
