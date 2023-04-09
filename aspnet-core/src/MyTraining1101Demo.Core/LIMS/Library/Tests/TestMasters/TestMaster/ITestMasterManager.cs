namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestMaster
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestMaster;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITestMasterManager : IDomainService
    {
        Task<PagedResultDto<TestMasterListDto>> GetPaginatedTestMasterListFromDB(TestMasterSearchDto input);

        Task<Guid> InsertOrUpdateTestMasterIntoDB(TestMasterInputDto input);

        Task<bool> DeleteTestMasterFromDB(Guid testMasterId);

        Task<TestMasterDto> GetTestMasterByIdFromDB(Guid testMasterId);

        Task<List<DropdownDto>> GetTestMasterListFromDB();

        Task<List<TestMasterListDto>> GetTestListFromDB();
    }
}
