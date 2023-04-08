namespace MyTraining1101Demo.LIMS.Library.Tests.TestMaster
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestMaster;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestMaster;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestSubApplications;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestVariables;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TestMasterAppService : MyTraining1101DemoAppServiceBase, ITestMasterAppService
    {
        private readonly ITestMasterManager _testMasterManager;
        private readonly ITestSubApplicationManager _testSubApplicationManager;
        private readonly ITestVariableManager _testVariableManager;

        public TestMasterAppService(ITestMasterManager testMasterManager, ITestSubApplicationManager testSubApplicationManager, ITestVariableManager testVariableManager)
        {
            _testMasterManager = testMasterManager;
            _testSubApplicationManager = testSubApplicationManager;
            _testVariableManager = testVariableManager;
        }


        public async Task<PagedResultDto<TestMasterListDto>> GetTestMasters(TestMasterSearchDto input)
        {
            try
            {
                var result = await this._testMasterManager.GetPaginatedTestMasterListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<Guid> InsertOrUpdateTest(TestMasterInputDto input)
        {
            try
            {
                var insertedOrUpdatedTestMasterId = await this._testMasterManager.InsertOrUpdateTestMasterIntoDB(input);

                if (insertedOrUpdatedTestMasterId != Guid.Empty)
                {
                    if (input.TestSubApplications != null && input.TestSubApplications.Count > 0)
                    {
                        input.TestSubApplications.ForEach(testSubApplicationItem =>
                        {
                            testSubApplicationItem.TestId = insertedOrUpdatedTestMasterId;
                        });
                        await this._testSubApplicationManager.BulkInsertOrUpdateTestSubApplications(input.TestSubApplications);
                    }

                    if (input.TestVariables != null && input.TestVariables.Count > 0)
                    {
                        input.TestVariables.ForEach(testVariableItem =>
                        {
                            testVariableItem.TestId = insertedOrUpdatedTestMasterId;
                        });
                        await this._testVariableManager.BulkInsertOrUpdateTestVariables(input.TestVariables);
                    }

                }
                return insertedOrUpdatedTestMasterId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteTestMasterData(Guid testMasterId)
        {
            try
            {
                var isTestMasterDeleted = await this._testMasterManager.DeleteTestMasterFromDB(testMasterId);

                //var isCustomerPOsDeleted = await this._customerPOManager.BulkDeleteCustomerPOs(customerId);

                //var isCustomerContactPersonsDeleted = await this._customerContactPersonManager.BulkDeleteCustomerContactPersons(customerId);

                //var isCustomerDataDeleted = await this._customerMasterManager.DeleteCustomerFromDB(customerId);

                return isTestMasterDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<TestMasterDto> GetTestMasterById(Guid testMasterId)
        {
            try
            {
                var testMasterItem = await this._testMasterManager.GetTestMasterByIdFromDB(testMasterId);

                if (testMasterItem.Id != Guid.Empty)
                {
                    testMasterItem.TestSubApplications = await this._testSubApplicationManager.GetTestSubApplicationListFromDB(testMasterItem.Id);
                    testMasterItem.TestVariables = await this._testVariableManager.GetTestVariableListFromDB(testMasterItem.Id);
                 }

                return testMasterItem;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<List<DropdownDto>> GetTestMasterList()
        {
            try
            {
                var response = await this._testMasterManager.GetTesMasterListFromDB();
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<List<TestMasterListDto>> GetTestList()
        {
            try
            {
                var response = await this._testMasterManager.GetTestListFromDB();
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
