namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanMaster
{
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestMaster;
    using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanTestMaster;
    using System;
    using System.Collections.Generic;

    public class TestPlanMasterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<TestPlanTestMasterDto> TestPlanTestMasters { get; set; } 

        public virtual Guid? ApplicationsId { get; set; }

        public virtual Guid? StandardReferenceId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
