namespace MyTraining1101Demo.LIMS.Library.Tests.Technique
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Technique.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public class TechniqueAppService : MyTraining1101DemoAppServiceBase, ITechniqueAppService
    {
        private readonly ITechniqueManager _techniqueManager;

        public TechniqueAppService(
          ITechniqueManager techniqueManager
         )
        {
            _techniqueManager = techniqueManager;
        }


        public async Task<PagedResultDto<TechniqueDto>> GetTechniques(TechniqueSearchDto input)
        {
            try
            {
                var result = await this._techniqueManager.GetPaginatedTechniqueFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<ResponseDto> InsertOrUpdateTechnique(TechniqueInputDto input)
        {
            try
            {
                var insertedOrUpdatedTechniqueId = await this._techniqueManager.InsertOrUpdateTechniqueIntoDB(input);

                return insertedOrUpdatedTechniqueId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteTechnique(Guid techniquelId)
        {
            try
            {
                var isTechniqueDeleted = await this._techniqueManager.DeleteTechniqueFromDB(techniquelId);
                return isTechniqueDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<TechniqueDto> GetTechniqueById(Guid techniquelId)
        {
            try
            {
                var response = await this._techniqueManager.GetTechniqueByIdFromDB(techniquelId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> RestoreTechnique(Guid techniquelId)
        {
            try
            {
                var response = await this._techniqueManager.RestoreTechnique(techniquelId);
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
