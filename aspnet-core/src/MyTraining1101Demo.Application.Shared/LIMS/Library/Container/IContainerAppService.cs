namespace MyTraining1101Demo.LIMS.Library.Container
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Container.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public interface IContainerAppService
    {
        Task<PagedResultDto<ContainerDto>> GetContainers(ContainerSearchDto input);
        Task<ResponseDto> InsertOrUpdateContainer(ContainerInputDto input);

        Task<bool> DeleteContainer(Guid containerId);

        Task<ContainerDto> GetContainerById(Guid containerId);

        Task<bool> RestoreContainer(Guid containerId);
    }
}
