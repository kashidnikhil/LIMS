namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplication
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.SubApplications;
    using MyTraining1101Demo.LIMS.Library.Tests.SubApplications.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SubApplicationAppService : MyTraining1101DemoAppServiceBase, ISubApplicationAppService
    {
        private readonly ISubApplicationManager _subApplicationManager;


        public SubApplicationAppService(
          ISubApplicationManager subApplicationManager
         )
        {
            _subApplicationManager = subApplicationManager;
        }


        public async Task<PagedResultDto<SubApplicationListDto>> GetSubApplications(SubApplicationSearchDto
            input)
        {
            try
            {
                var result = await this._subApplicationManager.GetPaginatedSubApplicationsListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<ResponseDto> InsertOrUpdateSubApplication(SubApplicationInputDto input)
        {
            try
            {
                var insertedOrUpdatedSubApplicationId = await this._subApplicationManager.InsertOrUpdateSubApplicationIntoDB(input);

                return insertedOrUpdatedSubApplicationId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteSubApplication(Guid subApplicationId)
        {
            try
            {
                var isSubApplicationDeleted = await this._subApplicationManager.DeleteSubApplicationFromDB(subApplicationId);
                return isSubApplicationDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<List<SubApplicationDto>> GetSubApplicationList(Guid? applicationId)
        {
            try
            {
                var response = await this._subApplicationManager.GetSubApplicationListFromDB(applicationId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<SubApplicationDto> GetSubApplicationById(Guid subApplicationId)
        {
            try
            {
                var response = await this._subApplicationManager.GetSubApplicationByIdFromDB(subApplicationId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> RestoreSubApplication(Guid subApplicationId)
        {
            try
            {
                var response = await this._subApplicationManager.RestoreSubApplication(subApplicationId);
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
