using Abp.Domain.Entities.Auditing;
using MyTraining1101Demo.LIMS.Library.Tests.SubApplications;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters
{
    [Table("TestVariable")]
    public class TestVariable : FullAuditedEntity<Guid>
    {
       public string Variable { get; set; }

       public string Description { get; set; }

        public virtual Guid? TestId { get; set; }
        public virtual Test Test { get; set; }
    }
}
