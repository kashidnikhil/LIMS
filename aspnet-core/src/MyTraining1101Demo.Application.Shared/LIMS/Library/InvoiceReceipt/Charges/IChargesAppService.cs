namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface IChargesAppService
    {
        Task<PagedResultDto<ChargesDto>> GetCharges(ChargesSearchDto input);
        Task<ResponseDto> InsertOrUpdateCharges(ChargesInputDto input);

        Task<bool> DeleteCharge(Guid chargeId);

        Task<bool> RestoreCharges(Guid chargeId);

        Task<ChargesDto> GetChargesById(Guid chargeId);
    }
}
