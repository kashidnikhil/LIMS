namespace MyTraining1101Demo.LIMS.Library.Tests.StandardReference
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardReference.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface IStandardReferenceAppService
    {
        Task<PagedResultDto<StandardReferenceDto>> GetStandardReferences(StandardReferenceSearchDto input);
        Task<ResponseDto> InsertOrUpdateStandardReference(StandardReferenceInputDto input);

        Task<bool> DeleteStandardReference(Guid standardReferenceId);

        Task<StandardReferenceDto> GetStandardReferenceById(Guid standardReferenceId);

        Task<bool> RestoreStandardReference(Guid standardReferenceId);
    }
}
