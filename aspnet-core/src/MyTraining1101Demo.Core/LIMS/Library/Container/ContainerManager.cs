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
    using MyTraining1101Demo.LIMS.Shared;
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
                    .Where(x => !x.IsDeleted)
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
        public async Task<ResponseDto> InsertOrUpdateContainerIntoDB(ContainerInputDto input)
        {
            try
            {
                Guid containerId = Guid.Empty;
                var containerItem = await this._containerRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == input.Name.ToLower().Trim());
                if (containerItem != null)
                {
                    if (input.Id != containerItem.Id)
                    {
                        return new ResponseDto
                        {
                            Id = input.Id == Guid.Empty ? null : input.Id,
                            Name = containerItem.Name,
                            IsExistingDataAlreadyDeleted = containerItem.IsDeleted,
                            DataMatchFound = true,
                            RestoringItemId = containerItem.Id
                        };
                    }
                    else {
                        containerItem.Name = input.Name;
                        containerItem.Description = input.Description;
                        containerId = await this._containerRepository.InsertOrUpdateAndGetIdAsync(containerItem);
                        return new ResponseDto
                        {
                            Id = containerId,
                            DataMatchFound = false
                        };
                    }

                }
                else
                {
                    var mappedContainerItem = ObjectMapper.Map<Container>(input);
                    containerId = await this._containerRepository.InsertOrUpdateAndGetIdAsync(mappedContainerItem);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return new ResponseDto
                    {
                        Id = containerId,
                        DataMatchFound = false
                    };
                }
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

        public async Task<bool> RestoreContainer(Guid containerId)
        {
            try
            {
                var containerItem = await this._containerRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == containerId);
                containerItem.IsDeleted = false;
                containerItem.DeleterUserId = null;
                containerItem.DeletionTime = null;
                await this._containerRepository.UpdateAsync(containerItem);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
