using Abp.Domain.Services;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestSubApplications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestSubApplications
{
    public interface ITestSubApplicationManager : IDomainService
    {
        Task<Guid> BulkInsertOrUpdateTestSubApplications(List<TestSubApplicationInputDto> testSubApplicationInputList);

        Task<bool> BulkDeleteTestSubApplications(Guid testMasterId);

        Task<IList<TestSubApplicationDto>> GetTestSubApplicationListFromDB(Guid testMasterId);
    }
}
