namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestMaster
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerMasters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class TestMasterManager : MyTraining1101DemoDomainServiceBase, ITestMasterManager
    {
        private readonly IRepository<Test, Guid> _testMasterRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public TestMasterManager(
           IRepository<Test, Guid> testMasterRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _testMasterRepository = testMasterRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<TestMasterDto>> GetPaginatedTestMasterListFromDB(TestMasterSearchDto input)
        {
            try
            {
                var testMasterQuery = this._testMasterRepository.GetAll()
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()) || item.Application.Name.ToLower().Contains(input.SearchString.ToLower()));


                var totalCount = await testMasterQuery.CountAsync();
                var items = await testMasterQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<TestMasterDto>(
                totalCount,
                ObjectMapper.Map<List<TestMasterDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdateTestMasterIntoDB(TestMasterInputDto input)
        {
            try
            {
                var mappedTestMasterItem = ObjectMapper.Map<Test>(input);
                var customerId = await this._testMasterRepository.InsertOrUpdateAndGetIdAsync(mappedTestMasterItem);
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
        public async Task<bool> DeleteTestMasterFromDB(Guid testMasterId)
        {
            try
            {
                var testMasterItem = await this._testMasterRepository.GetAsync(testMasterId);

                await this._testMasterRepository.DeleteAsync(testMasterItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<TestMasterDto> GetTestMasterByIdFromDB(Guid testMasterId)
        {
            try
            {
                var testMasterItem = await this._testMasterRepository.GetAsync(testMasterId);

                return ObjectMapper.Map<TestMasterDto>(testMasterItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }


    }
}
