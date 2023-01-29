namespace MyTraining1101Demo.LIMS.Library.Container
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Container.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class ContainerManager : MyTraining1101DemoDomainServiceBase, IContainerManager
    {
        private readonly IRepository<Container, Guid> _containerRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public ContainerManager(
           IRepository<Container, Guid> containerRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _containerRepository = containerRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<ContainerDto>> GetPaginatedContainerListFromDB(ContainerSearchDto input)
        {
            try
            {
                var containerQuery = this._containerRepository.GetAll()
                    .Where(x => x.IsDeleted == false)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await containerQuery.CountAsync();
                var items = await containerQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<ContainerDto>(
                totalCount,
                ObjectMapper.Map<List<ContainerDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateContainerIntoDB(ContainerInputDto input)
        {
            try
            {
                var mappedContainerItem = ObjectMapper.Map<Container>(input);
                var containerId = await this._containerRepository.InsertOrUpdateAndGetIdAsync(mappedContainerItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return containerId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteContainerFromDB(Guid containerId)
        {
            try
            {
                var containerItem = await this._containerRepository.GetAsync(containerId);

                await this._containerRepository.DeleteAsync(containerItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<ContainerDto> GetContainerByIdFromDB(Guid containerId)
        {
            try
            {
                var containerItem = await this._containerRepository.GetAsync(containerId);

                return ObjectMapper.Map<ContainerDto>(containerItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
