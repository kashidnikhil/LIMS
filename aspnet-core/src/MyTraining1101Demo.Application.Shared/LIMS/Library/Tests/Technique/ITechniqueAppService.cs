namespace MyTraining1101Demo.LIMS.Library.Tests.Technique
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Technique.Dto;
    using System;
    using System.Threading.Tasks;

    public interface ITechniqueAppService
    {
        Task<PagedResultDto<TechniqueDto>> GetTechniques(TechniqueSearchDto input);
        Task<Guid> InsertOrUpdateTechnique(TechniqueInputDto input);

        Task<bool> DeleteTechnique(Guid techniqueId);

        Task<TechniqueDto> GetTechniqueById(Guid techniqueId);
    }
}
