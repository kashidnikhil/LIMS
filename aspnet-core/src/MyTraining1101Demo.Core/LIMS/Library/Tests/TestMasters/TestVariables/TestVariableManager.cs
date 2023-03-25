using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using MyTraining1101Demo.Configuration;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestVariables
{
    public class TestVariableManager : MyTraining1101DemoDomainServiceBase, ITestVariableManager
    {
        private readonly IRepository<TestVariable, Guid> _testVariableRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public TestVariableManager(
           IRepository<TestVariable, Guid> testVariableRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _testVariableRepository = testVariableRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }
        public async Task<Guid> BulkInsertOrUpdateTestVariables(List<TestVariableInputDto> testVariableInputList)
        {
            try
            {
                Guid testId = Guid.Empty;
                var mappedTestVariables = ObjectMapper.Map<List<TestVariable>>(testVariableInputList);
                for (int i = 0; i < mappedTestVariables.Count; i++)
                {
                    testId = (Guid)mappedTestVariables[i].TestId;
                    await this.InsertOrUpdateTestVariableIntoDB(mappedTestVariables[i]);
                }
                return testId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [UnitOfWork]
        private async Task InsertOrUpdateTestVariableIntoDB(TestVariable input)
        {
            try
            {
                var testVariableId = await this._testVariableRepository.InsertOrUpdateAndGetIdAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> BulkDeleteTestVariables(Guid testId)
        {

            try
            {
                var testVariables = await this.GetTestVariableListFromDB(testId);

                if (testVariables.Count > 0)
                {
                    for (int i = 0; i < testVariables.Count; i++)
                    {
                        await this.DeleteTestVariableFromDB(testVariables[i].Id);
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
        private async Task DeleteTestVariableFromDB(Guid testVariableId)
        {
            try
            {
                var testVariableItem = await this._testVariableRepository.GetAsync(testVariableId);

                await this._testVariableRepository.DeleteAsync(testVariableItem);

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<IList<TestVariableDto>> GetTestVariableListFromDB(Guid testMasterId)
        {
            try
            {
                var testVariableQuery = this._testVariableRepository.GetAll()
                    .Where(x => !x.IsDeleted && x.TestId == testMasterId);

                return new List<TestVariableDto>(ObjectMapper.Map<List<TestVariableDto>>(testVariableQuery));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }
    }
}
