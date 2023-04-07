namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanMaster
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanMaster;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;

    using System.Threading.Tasks;
    public class TestPlanMasterManager : MyTraining1101DemoDomainServiceBase, ITestPlanMasterManager
    {
        private readonly IRepository<TestPlan, Guid> _testPlanMasterRepository;

        public TestPlanMasterManager(IRepository<TestPlan, Guid> testPlanMasterRepository)
        {
            _testPlanMasterRepository = testPlanMasterRepository;
        }

        public async Task<PagedResultDto<TestPlanMasterDto>> GetPaginatedTestPlanMasterListFromDB(TestPlanMasterSearchDto input)
        {
            try
            {
                var testMasterQuery = this._testPlanMasterRepository.GetAll()
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));


                var totalCount = await testMasterQuery.CountAsync();
                var items = await testMasterQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<TestPlanMasterDto>(
                totalCount,
                ObjectMapper.Map<List<TestPlanMasterDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateTestPlanMasterIntoDB(TestPlanMasterInputDto input)
        {
            try
            {
                var mappedTestPlanMasterItem = ObjectMapper.Map<TestPlan>(input);
                var customerId = await this._testPlanMasterRepository.InsertOrUpdateAndGetIdAsync(mappedTestPlanMasterItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return customerId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeleteTestPlanMasterFromDB(Guid testPlanMasterId)
        {
            try
            {
                var testPlanMasterItem = await this._testPlanMasterRepository.GetAsync(testPlanMasterId);

                await this._testPlanMasterRepository.DeleteAsync(testPlanMasterItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<TestPlanMasterDto> GetTestPlanMasterByIdFromDB(Guid testPlanMasterId)
        {
            try
            {
                var testPlanMasterItem = await this._testPlanMasterRepository.GetAsync(testPlanMasterId);

                return ObjectMapper.Map<TestPlanMasterDto>(testPlanMasterItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        //public async Task<List<DropdownDto>> GetTestPlanMasterListFromDB()
        //{
        //    try
        //    {
        //        var testPlanMasterItems = await this._testPlanMasterRepository.GetAll()
        //            .Where(testMasterItem => !testMasterItem.IsDeleted)
        //            .ToListAsync();

        //        return ObjectMapper.Map<List<DropdownDto>>(testPlanMasterItems);

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //        throw ex;
        //    }
        //}
    }
}
