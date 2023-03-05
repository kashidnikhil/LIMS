namespace MyTraining1101Demo.LIMS.Library.Container
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Container.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public class ContainerAppService : MyTraining1101DemoAppServiceBase, IContainerAppService
    {
        private readonly IContainerManager _containerManager;


        public ContainerAppService(
          IContainerManager containerManager
         )
        {
            _containerManager = containerManager;
        }


        public async Task<PagedResultDto<ContainerDto>> GetContainers(ContainerSearchDto input)
        {
            try
            {
                var result = await this._containerManager.GetPaginatedContainerListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<ResponseDto> InsertOrUpdateContainer(ContainerInputDto input)
        {
            try
            {
                var insertedOrUpdatedContainerId = await this._containerManager.InsertOrUpdateContainerIntoDB(input);

                return insertedOrUpdatedContainerId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteContainer(Guid containerId)
        {
            try
            {
                var isApplicationDeleted = await this._containerManager.DeleteContainerFromDB(containerId);
                return isApplicationDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<ContainerDto> GetContainerById(Guid containerId)
        {
            try
            {
                var response = await this._containerManager.GetContainerByIdFromDB(containerId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> RestoreContainer(Guid containerId)
        {
            try
            {
                var response = await this._containerManager.RestoreContainer(containerId);
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
