using Abp.Domain.Entities.Auditing;
using MyTraining1101Demo.LIMS.Library.Tests.SubApplications;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters
{
    [Table("TestSubApplication")]
    public class TestSubApplication : FullAuditedEntity<Guid>
    {
        public bool IsMOEF { get; set; }
        public bool IsNABL { get; set; }

        public virtual Guid? TestId { get; set; }
        public virtual Test Test { get; set; }

        public virtual Guid? SubApplicationId { get; set; }
        public virtual SubApplication SubApplication { get; set; }
    }
}
