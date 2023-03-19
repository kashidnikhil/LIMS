namespace MyTraining1101Demo.LIMS.Library.Tests.Unit
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Unit.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Units;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public class UnitAppService : MyTraining1101DemoAppServiceBase, IUnitAppService
    {
        private readonly IUnitManager _unitManager;

        public UnitAppService(
          IUnitManager unitManager
         )
        {
            _unitManager = unitManager;
        }


        public async Task<PagedResultDto<UnitDto>> GetUnits(UnitSearchDto input)
        {
            try
            {
                var result = await this._unitManager.GetPaginatedUnitFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<ResponseDto> InsertOrUpdateUnit(UnitInputDto input)
        {
            try
            {
                var insertedOrUpdatedUnitId = await this._unitManager.InsertOrUpdateUnitIntoDB(input);

                return insertedOrUpdatedUnitId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteUnit(Guid unitId)
        {
            try
            {
                var isUnitDeleted = await this._unitManager.DeleteUnitFromDB(unitId);
                return isUnitDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<UnitDto> GetUnitById(Guid unitId)
        {
            try
            {
                var response = await this._unitManager.GetUnitByIdFromDB(unitId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> RestoreUnit(Guid unitId)
        {
            try
            {
                var response = await this._unitManager.RestoreUnit(unitId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
