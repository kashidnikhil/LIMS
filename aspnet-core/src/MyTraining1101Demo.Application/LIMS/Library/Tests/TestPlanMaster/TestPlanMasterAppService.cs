using Abp.Application.Services.Dto;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestMaster;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestMaster;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestSubApplications;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestVariables;
using MyTraining1101Demo.LIMS.Library.Tests.TestPlans;
using MyTraining1101Demo.LIMS.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanMaster;
using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanTestMasters;
using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanMaster;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlanMaster
{
    public class TestPlanMasterAppService : MyTraining1101DemoAppServiceBase, ITestPlanMasterAppService
    {
        private readonly ITestPlanMasterManager _testPlanMasterManager;
        private readonly ITestPlanTestMasterManager _testPlanTestMasterManager;
   
        public TestPlanMasterAppService(ITestPlanMasterManager testPlanMasterManager, ITestPlanTestMasterManager testPlanTestMasterManager)
        {
            _testPlanMasterManager = testPlanMasterManager;
            _testPlanTestMasterManager = testPlanTestMasterManager;
        }


        public async Task<PagedResultDto<TestPlanMasterDto>> GetTestPlanMasters(TestPlanMasterSearchDto input)
        {
            try
            {
                var result = await this._testPlanMasterManager.GetPaginatedTestPlanMasterListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
      
        public async Task<Guid> InsertOrUpdateTestPlan(TestPlanMasterInputDto input)
        {
            try
            {
                var insertedOrUpdatedTestPlanMasterId = await this._testPlanMasterManager.InsertOrUpdateTestPlanMasterIntoDB(input);

                if (insertedOrUpdatedTestPlanMasterId != Guid.Empty)
                {
                   

                    if (input.TestPlanTestMasters != null && input.TestPlanTestMasters.Count > 0)
                    {
                        input.TestPlanTestMasters.ForEach(testVariableItem =>
                        {
                            testVariableItem.TestPlanId = insertedOrUpdatedTestPlanMasterId;
                        });
                        await this._testPlanTestMasterManager.BulkInsertOrUpdateTestPlanTestMaster(input.TestPlanTestMasters);
                    }

                }
                return insertedOrUpdatedTestPlanMasterId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteTestPlanMasterData(Guid testPlanMasterId)
        {
            try
            {
                var isTestPlanMasterDeleted = await this._testPlanMasterManager.DeleteTestPlanMasterFromDB(testPlanMasterId);

                //var isCustomerPOsDeleted = await this._customerPOManager.BulkDeleteCustomerPOs(customerId);

                //var isCustomerContactPersonsDeleted = await this._customerContactPersonManager.BulkDeleteCustomerContactPersons(customerId);

                //var isCustomerDataDeleted = await this._customerMasterManager.DeleteCustomerFromDB(customerId);

                return isTestPlanMasterDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<TestPlanMasterDto> GetTestPlanMasterById(Guid testPlanMasterId)
        {
            try
            {
                var testPlanMasterItem = await this._testPlanMasterManager.GetTestPlanMasterByIdFromDB(testPlanMasterId);

                if (testPlanMasterItem.Id != Guid.Empty)
                {
                    testPlanMasterItem.TestPlanTestMasterList = await this._testPlanTestMasterManager.GetTestPlanTestMasterListFromDB(testPlanMasterItem.Id);
                }

                return testPlanMasterItem;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
