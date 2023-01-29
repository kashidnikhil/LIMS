namespace MyTraining1101Demo.LIMS.Library.Container
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Container.Dto;
    using System;
    using System.Threading.Tasks;

    public interface IContainerManager : IDomainService
    {
        Task<PagedResultDto<ContainerDto>> GetPaginatedContainerListFromDB(ContainerSearchDto input);
        Task<Guid> InsertOrUpdateContainerIntoDB(ContainerInputDto input);

        Task<bool> DeleteContainerFromDB(Guid containerId);

        Task<ContainerDto> GetContainerByIdFromDB(Guid containerId);
    }
}
