namespace MyTraining1101Demo.LIMS.Library.Tests.StandardReference
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using System.Threading.Tasks;
    using System;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardReference.Dto;

    public interface IStandardReferenceManager : IDomainService
    {
        Task<PagedResultDto<StandardReferenceDto>> GetPaginatedStandardReferenceListFromDB(StandardReferenceSearchDto input);
        Task<Guid> InsertOrUpdateStandardReferenceIntoDB(StandardReferenceInputDto input);

        Task<bool> DeleteStandardReferenceFromDB(Guid standardReferenceId);

        Task<StandardReferenceDto> GetStandardReferenceByIdFromDB(Guid standardReferenceId);
    }
}
