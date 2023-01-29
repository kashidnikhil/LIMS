namespace MyTraining1101Demo.LIMS.Library.Tests.Unit
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.Unit.Dto;
    using System;
    using System.Threading.Tasks;

    public interface IUnitManager : IDomainService
    {
        Task<PagedResultDto<UnitDto>> GetPaginatedUnitFromDB(UnitSearchDto input);
        Task<Guid> InsertOrUpdateUnitIntoDB(UnitInputDto input);

        Task<bool> DeleteUnitFromDB(Guid unitId);

        Task<UnitDto> GetUnitByIdFromDB(Guid unitId);
    }
}
