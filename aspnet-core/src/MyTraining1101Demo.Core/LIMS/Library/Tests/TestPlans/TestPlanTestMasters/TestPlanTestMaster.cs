using Abp.Domain.Entities.Auditing;
using MyTraining1101Demo.LIMS.Library.Tests.StandardReferences;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters;
using MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanMaster;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanTestMasters
{
    [Table("TestPlanTestMaster")]
    public class TestPlanTestMaster : FullAuditedEntity<Guid>
    {
        public virtual Guid? TestPlanId { get; set; }
        public virtual TestPlan TestPlan { get; set; }

        public virtual Guid? TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}
