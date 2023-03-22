namespace MyTraining1101Demo.LIMS.Library.Tests.Techniques
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.Technique.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITechniqueManager : IDomainService
    {
        Task<PagedResultDto<TechniqueDto>> GetPaginatedTechniqueFromDB(TechniqueSearchDto input);
        Task<ResponseDto> InsertOrUpdateTechniqueIntoDB(TechniqueInputDto input);

        Task<bool> DeleteTechniqueFromDB(Guid techniqueId);

        Task<TechniqueDto> GetTechniqueByIdFromDB(Guid techniqueId);

        Task<IList<TechniqueDto>> GetTechniqueListFromDB();

        Task<bool> RestoreTechnique(Guid techniqueId);
    }
}
