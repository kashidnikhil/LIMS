namespace MyTraining1101Demo.LIMS.Library.Tests.StandardRemark
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.StandardRemark.Dto;
    using System;
    using System.Threading.Tasks;


    public class StandardRemarkAppService : MyTraining1101DemoAppServiceBase, IStandardRemarkAppService
    {
        private readonly IStandardRemarkManager _standardRemarkManager;

        public StandardRemarkAppService(
          IStandardRemarkManager standardRemarkManager
         )
        {
            _standardRemarkManager = standardRemarkManager;
        }


        public async Task<PagedResultDto<StandardRemarkDto>> GetStandardRemarks(StandardRemarkSearchDto input)
        {
            try
            {
                var result = await this._standardRemarkManager.GetPaginatedStandardRemarkListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<Guid> InsertOrUpdateStandardRemark(StandardRemarkInputDto input)
        {
            try
            {
                var insertedOrUpdatedStandardRemarkId = await this._standardRemarkManager.InsertOrUpdateStandardRemarkIntoDB(input);

                return insertedOrUpdatedStandardRemarkId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteStandardRemark(Guid standardRemarkId)
        {
            try
            {
                var isStandardRemarkDeleted = await this._standardRemarkManager.DeleteStandardRemarkFromDB(standardRemarkId);
                return isStandardRemarkDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<StandardRemarkDto> GetStandardRemarkById(Guid standardRemarkId)
        {
            try
            {
                var response = await this._standardRemarkManager.GetStandardRemarkByIdFromDB(standardRemarkId);
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
