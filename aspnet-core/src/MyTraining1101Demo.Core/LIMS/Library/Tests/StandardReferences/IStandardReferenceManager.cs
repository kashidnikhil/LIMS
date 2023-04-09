namespace MyTraining1101Demo.LIMS.Library.Tests.StandardReferences
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using System.Threading.Tasks;
    using System;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardReferences.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System.Collections.Generic;

    public interface IStandardReferenceManager : IDomainService
    {
        Task<PagedResultDto<StandardReferenceDto>> GetPaginatedStandardReferenceListFromDB(StandardReferenceSearchDto input);
        Task<ResponseDto> InsertOrUpdateStandardReferenceIntoDB(StandardReferenceInputDto input);

        Task<bool> DeleteStandardReferenceFromDB(Guid standardReferenceId);

        Task<StandardReferenceDto> GetStandardReferenceByIdFromDB(Guid standardReferenceId);

        Task<bool> RestoreStandardReference(Guid standardReferencelId);

        Task<List<DropdownDto>> GetStandardReferenceListFromDB();
    }
}
