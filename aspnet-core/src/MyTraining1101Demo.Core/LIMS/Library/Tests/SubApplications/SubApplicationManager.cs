namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplications
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Tests.SubApplications.Dto;
    using PayPalCheckoutSdk.Orders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class SubApplicationManager : MyTraining1101DemoDomainServiceBase, ISubApplicationManager
    {
        private readonly IRepository<SubApplication, Guid> _subApplicationsRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public SubApplicationManager(
           IRepository<SubApplication, Guid> subApplicationRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _subApplicationsRepository = subApplicationRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }


        public async Task<PagedResultDto<SubApplicationListDto>> GetPaginatedSubApplicationsListFromDB(SubApplicationSearchDto input)
        {
            try
            {
                var subApplicationQuery = this._subApplicationsRepository.GetAllIncluding(x=> x.Application)
                // .Include(x=> x.Applications)
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()) || item.Application.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await subApplicationQuery.CountAsync();
                var items = await subApplicationQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<SubApplicationListDto>(
                totalCount,
                ObjectMapper.Map<List<SubApplicationListDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateSubApplicationIntoDB(SubApplicationInputDto input)
        {
            try
            {
                var mappedSubApplicationItem = ObjectMapper.Map<SubApplication>(input);
                var subAppplicationId = await this._subApplicationsRepository.InsertOrUpdateAndGetIdAsync(mappedSubApplicationItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return subAppplicationId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteSubApplicationFromDB(Guid subApplicationId)
        {
            try
            {
                var subApplicationItem = await this._subApplicationsRepository.GetAsync(subApplicationId);

                await this._subApplicationsRepository.DeleteAsync(subApplicationItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<SubApplicationDto> GetSubApplicationByIdFromDB(Guid subApplicationId)
        {
            try
            {
                var subApplicationItem = await this._subApplicationsRepository.GetAsync(subApplicationId);

                return ObjectMapper.Map<SubApplicationDto>(subApplicationItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    
    }
}
