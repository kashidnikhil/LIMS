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
                    .Where(x=> x.IsDeleted == false)
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
        public async Task<Guid> InsertOrUpdateApplicationIntoDB(ApplicationInputDto input)
        {
            try
            {
                var mappedApplicationItem = ObjectMapper.Map<Applications>(input);
                var appplicationId = await this._applicationsRepository.InsertOrUpdateAndGetIdAsync(mappedApplicationItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return appplicationId;
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
    }
}
