namespace MyTraining1101Demo.LIMS.Library.Tests.Application
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ApplicationsAppService : MyTraining1101DemoAppServiceBase, IApplicationsAppService
    {
        private readonly IApplicationsManager _applicationsManager;


        public ApplicationsAppService(
          IApplicationsManager applicationsManager
         )
        {
            _applicationsManager = applicationsManager;
        }


        public async Task<PagedResultDto<ApplicationsDto>> GetApplications(ApplicationsSearchDto input) {
            try
            {
                var result = await this._applicationsManager.GetPaginatedApplicationsListFromDB(input);
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<ResponseDto> InsertOrUpdateApplication(ApplicationInputDto input) {
            try
            {
                var response = await this._applicationsManager.InsertOrUpdateApplicationIntoDB(input);

                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message,ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteApplication(Guid applicationId) {
            try
            {
                var isApplicationDeleted = await this._applicationsManager.DeleteApplicationFromDB(applicationId);
                return isApplicationDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<ApplicationsDto> GetApplicationById(Guid applicationId) { 
            try
            {
                var response = await this._applicationsManager.GetApplicationByIdFromDB(applicationId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<IList<ApplicationsDto>> GetApplicationList()
        {
            try
            {
                var response = await this._applicationsManager.GetApplicationsListFromDB();
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> RevokeApplication(Guid applicationId)
        {
            try
            {
                var response = await this._applicationsManager.RevokeApplication(applicationId);
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
