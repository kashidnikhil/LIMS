namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank.Dto;
    using System;
    using System.Threading.Tasks;

    public interface IBankAppService
    {
        Task<PagedResultDto<BankDto>> GetBanks(BankSearchDto input);
        Task<Guid> InsertOrUpdateBank(BankInputDto input);

        Task<bool> DeleteBank(Guid bankId);

        Task<BankDto> GetBankById(Guid bankId);
    }
}
