namespace MyTraining1101Demo.LIMS.Library.Tests.StandardReference
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using System.Threading.Tasks;
    using System;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardReference.Dto;
    using MyTraining1101Demo.LIMS.Shared;

    public interface IStandardReferenceManager : IDomainService
    {
        Task<PagedResultDto<StandardReferenceDto>> GetPaginatedStandardReferenceListFromDB(StandardReferenceSearchDto input);
        Task<ResponseDto> InsertOrUpdateStandardReferenceIntoDB(StandardReferenceInputDto input);

        Task<bool> DeleteStandardReferenceFromDB(Guid standardReferenceId);

        Task<StandardReferenceDto> GetStandardReferenceByIdFromDB(Guid standardReferenceId);

        Task<bool> RestoreStandardReference(Guid standardReferencelId);
    }
}
