namespace MyTraining1101Demo.LIMS.Library.Tests.Unit
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Unit.Dto;
    using System;
    using System.Threading.Tasks;

    public interface IUnitAppService
    {
        Task<PagedResultDto<UnitDto>> GetUnits(UnitSearchDto input);
        Task<Guid> InsertOrUpdateUnit(UnitInputDto input);

        Task<bool> DeleteUnit(Guid unitId);

        Task<UnitDto> GetUnitById(Guid unitId);
    }
}
