namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Bank.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;
    public interface IBankManager : IDomainService
    {
        Task<PagedResultDto<BankDto>> GetPaginatedBankListFromDB(BankSearchDto input);
        Task<ResponseDto> InsertOrUpdateBankIntoDB(BankInputDto input);

        Task<bool> DeleteBankFromDB(Guid bankId);

        Task<BankDto> GetBankByIdFromDB(Guid bankId);

        Task<bool> RestoreBank(Guid bankId);
    }
}
