using Abp.Domain.Entities.Auditing;
using MyTraining1101Demo.LIMS.Library.Tests.StandardReferences;
using MyTraining1101Demo.LIMS.Library.Tests.TestMasters;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanTestMasterMapping
{
    [Table("TestPlanTestMaster")]
    public class TestPlanTestMaster : FullAuditedEntity<Guid>
    {
        public virtual Guid? StandardReferenceId { get; set; }
        public virtual StandardReference Technique { get; set; }

        public virtual Guid? TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}
