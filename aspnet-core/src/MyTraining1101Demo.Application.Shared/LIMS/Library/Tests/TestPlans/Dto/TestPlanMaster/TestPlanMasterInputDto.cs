using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanTestMaster;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanMaster
{
    public class TestPlanMasterInputDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public Guid? ApplicationsId { get; set; }

        public Guid? StandardReferenceId { get; set; }

        public List<TestPlanTestMasterInputDto> TestPlanTestMasters { get; set; }
    }
}
