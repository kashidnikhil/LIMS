namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanTestMasters
{
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanTestMaster;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TestPlanTestMasterManager: MyTraining1101DemoDomainServiceBase, ITestPlanTestMasterManager
    {
        private readonly IRepository<TestPlanTestMaster, Guid> _testPlanMappingRepository;
      
        public TestPlanTestMasterManager(IRepository<TestPlanTestMaster, Guid> testPlanMappingRepository)
        {
            _testPlanMappingRepository = testPlanMappingRepository;
        }
        public async Task<Guid> BulkInsertOrUpdateTestPlanTestMaster(List<TestPlanTestMasterInputDto> testPlanTestMasterInputList)
        {
            try
            {
                Guid testPlanId = Guid.Empty;
                var mappedTestPlanTestMasters = ObjectMapper.Map<List<TestPlanTestMaster>>(testPlanTestMasterInputList);
                for (int i = 0; i < mappedTestPlanTestMasters.Count; i++)
                {
                    testPlanId = (Guid)mappedTestPlanTestMasters[i].TestId;
                    await this.InsertOrUpdateTestPlanTestMasterIntoDB(mappedTestPlanTestMasters[i]);
                }
                return testPlanId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [UnitOfWork]
        private async Task InsertOrUpdateTestPlanTestMasterIntoDB(TestPlanTestMaster input)
        {
            try
            {
                var testPlanTestMasterId = await this._testPlanMappingRepository.InsertOrUpdateAndGetIdAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> BulkDeleteTestPlanTestMaster(Guid testPlanId)
        {

            try
            {
                var testPlanTestMasters = await this.GetTestPlanTestMasterListFromDB(testPlanId);

                if (testPlanTestMasters.Count > 0)
                {
                    for (int i = 0; i < testPlanTestMasters.Count; i++)
                    {
                        await this.DeleteTestPlanTestMasterFromDB(testPlanTestMasters[i].Id);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [UnitOfWork]
        private async Task DeleteTestPlanTestMasterFromDB(Guid testPlanTestMasterId)
        {
            try
            {
                var testPlanTestMasterItem = await this._testPlanMappingRepository.GetAsync(testPlanTestMasterId);

                await this._testPlanMappingRepository.DeleteAsync(testPlanTestMasterItem);

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<IList<TestPlanTestMasterDto>> GetTestPlanTestMasterListFromDB(Guid testPlanId)
        {
            try
            {
                var testPlanTestMasterQuery = this._testPlanMappingRepository.GetAll()
                    .Where(x => !x.IsDeleted && x.TestPlanId == testPlanId);

                return new List<TestPlanTestMasterDto>(ObjectMapper.Map<List<TestPlanTestMasterDto>>(testPlanTestMasterQuery));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }
    }
}
