using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using MyTraining1101Demo.Configuration;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestSubApplications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestSubApplications
{
    public class TestSubApplicationManager : MyTraining1101DemoDomainServiceBase, ITestSubApplicationManager
    {
        private readonly IRepository<TestSubApplication, Guid> _testSubApplicationRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public TestSubApplicationManager(
           IRepository<TestSubApplication, Guid> testSubApplicationRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _testSubApplicationRepository = testSubApplicationRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }
        public async Task<Guid> BulkInsertOrUpdateTestSubApplications(List<TestSubApplicationInputDto> testSubApplicationInputList)
        {
            try
            {
                Guid testId = Guid.Empty;
                var mappedTestSubApplications = ObjectMapper.Map<List<TestSubApplication>>(testSubApplicationInputList);
                for (int i = 0; i < mappedTestSubApplications.Count; i++)
                {
                    testId = (Guid)mappedTestSubApplications[i].TestId;
                    await this.InsertOrUpdateTestSubApplicationIntoDB(mappedTestSubApplications[i]);
                }
                return testId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [UnitOfWork]
        private async Task InsertOrUpdateTestSubApplicationIntoDB(TestSubApplication input)
        {
            try
            {
                var customerAddressId = await this._testSubApplicationRepository.InsertOrUpdateAndGetIdAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> BulkDeleteTestSubApplications(Guid testMasterId)
        {

            try
            {
                var testSubApplications = await this.GetTestSubApplicationListFromDB(testMasterId);

                if (testSubApplications.Count > 0)
                {
                    for (int i = 0; i < testSubApplications.Count; i++)
                    {
                        await this.DeleteTestSubApplicationFromDB(testSubApplications[i].Id);
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
        private async Task DeleteTestSubApplicationFromDB(Guid testSubApplicationId)
        {
            try
            {
                var testSubApplicationItem = await this._testSubApplicationRepository.GetAsync(testSubApplicationId);

                await this._testSubApplicationRepository.DeleteAsync(testSubApplicationItem);

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<IList<TestSubApplicationDto>> GetTestSubApplicationListFromDB(Guid testMasterId)
        {
            try
            {
                var testSubApplicationQuery = this._testSubApplicationRepository.GetAllIncluding(x => x.SubApplication)
                    .Where(x => !x.IsDeleted && x.TestId == testMasterId);

                return new List<TestSubApplicationDto>(ObjectMapper.Map<List<TestSubApplicationDto>>(testSubApplicationQuery));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

    }
}
