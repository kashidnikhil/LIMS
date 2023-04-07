namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanTestMasters
{
    using Abp.Domain.Services;
    using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanTestMaster;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITestPlanTestMasterManager : IDomainService
    {
        Task<Guid> BulkInsertOrUpdateTestPlanTestMaster(List<TestPlanTestMasterInputDto> testPlanTestMasterInputList);

        Task<bool> BulkDeleteTestPlanTestMaster(Guid testPlanId);

        Task<IList<TestPlanTestMasterDto>> GetTestPlanTestMasterListFromDB(Guid testPlanId);
    }
}
