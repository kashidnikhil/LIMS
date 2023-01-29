namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges.Dto;
    using System;
    using System.Threading.Tasks;


    public interface IChargesManager : IDomainService
    {
        Task<PagedResultDto<ChargesDto>> GetPaginatedChargeListFromDB(ChargesSearchDto input);
        Task<Guid> InsertOrUpdateChargeIntoDB(ChargesInputDto input);

        Task<bool> DeleteChargeFromDB(Guid chargeId);

        Task<ChargesDto> GetChargeByIdFromDB(Guid chargeId);
    }
}
