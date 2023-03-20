﻿namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestMaster;
    using System;
    using System.Threading.Tasks;

    public interface ITestMasterAppService : IDomainService
    {
        Task<PagedResultDto<TestMasterListDto>> GetTestMasters(TestMasterSearchDto input);

        Task<Guid> InsertOrUpdateTest(TestMasterInputDto input);

        Task<bool> DeleteTestMasterData(Guid testMasterId);

        Task<TestMasterDto> GetTestMasterById(Guid testMasterId);
    }
}
