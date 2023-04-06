using Abp.Domain.Entities.Auditing;
using MyTraining1101Demo.LIMS.Library.Tests.Application;
using MyTraining1101Demo.LIMS.Library.Tests.StandardReferences;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.TestPlanMaster
{
    [Table("TestPlan")]
    public class TestPlan : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }

        public virtual Guid? ApplicationsId { get; set; }
        public virtual Applications Applications { get; set; }

        public virtual Guid? StandardReferenceId { get; set; }
        public virtual StandardReference Technique { get; set; }
    }
}
