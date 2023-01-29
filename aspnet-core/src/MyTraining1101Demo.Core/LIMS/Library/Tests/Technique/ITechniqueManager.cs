namespace MyTraining1101Demo.LIMS.Library.Tests.Technique
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardReference.Dto;

    using System.Threading.Tasks;

    using System;
    using MyTraining1101Demo.LIMS.Library.Tests.Technique.Dto;

    public interface ITechniqueManager : IDomainService
    {
        Task<PagedResultDto<TechniqueDto>> GetPaginatedTechniqueFromDB(TechniqueSearchDto input);
        Task<Guid> InsertOrUpdateTechniqueIntoDB(TechniqueInputDto input);

        Task<bool> DeleteTechniqueFromDB(Guid techniqueId);

        Task<TechniqueDto> GetTechniqueByIdFromDB(Guid techniqueId);
    }
}
