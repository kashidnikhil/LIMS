namespace MyTraining1101Demo.LIMS.Library.Tests.StandardReference
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardReference.Dto;
    using System;
    using System.Threading.Tasks;

    public interface IStandardReferenceAppService
    {
        Task<PagedResultDto<StandardReferenceDto>> GetStandardReferences(StandardReferenceSearchDto input);
        Task<Guid> InsertOrUpdateStandardReference(StandardReferenceInputDto input);

        Task<bool> DeleteStandardReference(Guid standardReferenceId);

        Task<StandardReferenceDto> GetStandardReferenceById(Guid standardReferenceId);
    }
}
