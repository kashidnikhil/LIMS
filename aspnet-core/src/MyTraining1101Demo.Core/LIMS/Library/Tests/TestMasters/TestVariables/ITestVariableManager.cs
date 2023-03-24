using Abp.Domain.Services;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestVariables
{
    public interface ITestVariableManager : IDomainService
    {
        Task<Guid> BulkInsertOrUpdateTestVariables(List<TestVariableInputDto> testVariableInputList);

        Task<bool> BulkDeleteTestVariables(Guid testId);

        Task<IList<TestVariableDto>> GetTestVariableListFromDB(Guid testMasterId);


    }
}
