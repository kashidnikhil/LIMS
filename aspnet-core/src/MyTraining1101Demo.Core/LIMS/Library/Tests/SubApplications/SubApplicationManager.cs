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
    using MyTraining1101Demo.LIMS.Shared;
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
        public async Task<ResponseDto> InsertOrUpdateSubApplicationIntoDB(SubApplicationInputDto input)
        {
            try
            {
                Guid subAppplicationId = Guid.Empty;
                var subApplicationItem = await this._subApplicationsRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Name.ToLower().Trim() == input.Name.ToLower().Trim());
                if (subApplicationItem != null && input.Id != subApplicationItem.Id)
                {
                    return new ResponseDto
                    {
                        Id = input.Id == Guid.Empty ? null : input.Id,
                        Name = subApplicationItem.Name,
                        IsExistingDataAlreadyDeleted = subApplicationItem.IsDeleted,
                        DataMatchFound = true,
                        RestoringItemId = subApplicationItem.Id
                    };
                }
                else
                {
                    var mappedSubApplicationItem = ObjectMapper.Map<SubApplication>(input);
                    subAppplicationId = await this._subApplicationsRepository.InsertOrUpdateAndGetIdAsync(mappedSubApplicationItem);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return new ResponseDto
                    {
                        Id = subAppplicationId,
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

        public async Task<List<SubApplicationDto>> GetSubApplicationListFromDB(Guid applicationId)
        {
            try
            {
                var subApplicationItems = await this._subApplicationsRepository.GetAll().Where(x=> x.ApplicationId == applicationId && !x.IsDeleted).ToListAsync();

                return ObjectMapper.Map <List<SubApplicationDto>> (subApplicationItems);

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
                var subApplicationItem = await this._subApplicationsRepository.GetAll().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == subApplicationId);
                subApplicationItem.IsDeleted = false;
                subApplicationItem.DeleterUserId = null;
                subApplicationItem.DeletionTime = null;
                await this._subApplicationsRepository.UpdateAsync(subApplicationItem);

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
