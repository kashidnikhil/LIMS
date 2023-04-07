namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanMaster
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanMaster;
    using System;
    using System.Threading.Tasks;

    public interface ITestPlanMasterManager : IDomainService
    {
        Task<PagedResultDto<TestPlanMasterDto>> GetPaginatedTestPlanMasterListFromDB(TestPlanMasterSearchDto input);

        Task<Guid> InsertOrUpdateTestPlanMasterIntoDB(TestPlanMasterInputDto input);

        Task<bool> DeleteTestPlanMasterFromDB(Guid testPlanMasterId);

        Task<TestPlanMasterDto> GetTestPlanMasterByIdFromDB(Guid testPlanMasterId);
    }
}
