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
    using MyTraining1101Demo.LIMS.Library.Tests.SubApplications.Dto;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestMaster;
    using MyTraining1101Demo.LIMS.Shared;
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

        public async Task<PagedResultDto<TestMasterListDto>> GetPaginatedTestMasterListFromDB(TestMasterSearchDto input)
        {
            try
            {
                var testMasterQuery = this._testMasterRepository.GetAllIncluding(x=> x.Unit)
                    .Include(x=> x.Application)
                    .Include(x=> x.Technique)
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()) || item.Application.Name.ToLower().Contains(input.SearchString.ToLower()));


                var totalCount = await testMasterQuery.CountAsync();
                var items = await testMasterQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<TestMasterListDto>(
                totalCount,
                ObjectMapper.Map<List<TestMasterListDto>>(items));
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

        public async Task<List<DropdownDto>> GetTesMasterListFromDB()
        {
            try
            {
                var testMasterItems = await this._testMasterRepository.GetAll()
                    .Where(testMasterItem => !testMasterItem.IsDeleted)
                    .ToListAsync();

                return ObjectMapper.Map<List<DropdownDto>>(testMasterItems);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<List<TestMasterDto>> GetTestListFromDB()
        {
            try
            {
                var testMasterItems = await this._testMasterRepository.GetAll()
                    .Where(testMasterItem => !testMasterItem.IsDeleted)
                    .ToListAsync();

                return ObjectMapper.Map<List<TestMasterDto>>(testMasterItems);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
