namespace MyTraining1101Demo.LIMS.Library.Tests.Application
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Tests.Application.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class ApplicationsManager : MyTraining1101DemoDomainServiceBase, IApplicationsManager
    {
        private readonly IRepository<Applications, Guid> _applicationsRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public ApplicationsManager(
           IRepository<Applications, Guid> applicationsRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _applicationsRepository = applicationsRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }


        public async Task<PagedResultDto<ApplicationsDto>> GetPaginatedApplicationsListFromDB(ApplicationsSearchDto input)
        {
            try
            {
                var applicationQuery = this._applicationsRepository.GetAll()
                    .Where(x=> !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await applicationQuery.CountAsync();
                var items = await applicationQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<ApplicationsDto>(
                totalCount,
                ObjectMapper.Map<List<ApplicationsDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<ResponseDto> InsertOrUpdateApplicationIntoDB(ApplicationInputDto input)
        {
            try
            {
                Guid applicationId = Guid.Empty;
                var applicationItem = await this._applicationsRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == input.Name.ToLower().Trim());
                if (applicationItem != null)
                {

                    if (input.Id != applicationItem.Id)
                    {
                        return new ResponseDto
                        {
                            Id = input.Id == Guid.Empty ? null : input.Id,
                            Name = applicationItem.Name,
                            IsExistingDataAlreadyDeleted = applicationItem.IsDeleted,
                            DataMatchFound = true,
                            RestoringItemId = applicationItem.Id
                        };
                    }
                    else {
                        applicationItem.Name = input.Name;
                        applicationItem.Description = input.Description;
                        applicationId = await this._applicationsRepository.InsertOrUpdateAndGetIdAsync(applicationItem);
                        return new ResponseDto
                        {
                            Id = applicationId,
                            DataMatchFound = false
                        };
                    }
                    
                }
                else {
                    var mappedApplicationItem = ObjectMapper.Map<Applications>(input);
                    applicationId = await this._applicationsRepository.InsertOrUpdateAndGetIdAsync(mappedApplicationItem);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return new ResponseDto
                    {
                        Id = applicationId,
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
        public async Task<bool> DeleteApplicationFromDB(Guid applicationId)
        {
            try
            {
                var applicationItem = await this._applicationsRepository.GetAsync(applicationId);

                await this._applicationsRepository.DeleteAsync(applicationItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<ApplicationsDto> GetApplicationByIdFromDB(Guid applicationId)
        {
            try
            {
                var applicationItem = await this._applicationsRepository.GetAsync(applicationId);

                return ObjectMapper.Map<ApplicationsDto>(applicationItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<IList<ApplicationsDto>> GetApplicationsListFromDB()
        {
            try
            {
                var applicationsQuery = this._applicationsRepository.GetAll()
                    .Where(x => !x.IsDeleted);
          
                return new List<ApplicationsDto>(ObjectMapper.Map<List<ApplicationsDto>>(applicationsQuery));
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
                var applicationItem = await this._applicationsRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == applicationId);
                applicationItem.IsDeleted = false;
                applicationItem.DeleterUserId = null;
                applicationItem.DeletionTime = null;
                await this._applicationsRepository.UpdateAsync(applicationItem);

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
