namespace MyTraining1101Demo.LIMS.Library.Tests.Units
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.Unit.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface IUnitManager : IDomainService
    {
        Task<PagedResultDto<UnitDto>> GetPaginatedUnitFromDB(UnitSearchDto input);
        Task<ResponseDto> InsertOrUpdateUnitIntoDB(UnitInputDto input);

        Task<bool> DeleteUnitFromDB(Guid unitId);

        Task<UnitDto> GetUnitByIdFromDB(Guid unitId);

        Task<bool> RestoreUnit(Guid unitId);
    }
}
