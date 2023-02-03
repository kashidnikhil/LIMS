namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplications
{
    using Abp.Domain.Entities.Auditing;
    using MyTraining1101Demo.LIMS.Library.Tests.Application;
    using System;

    public class SubApplication : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }

        public virtual Guid? ApplicationId { get; set; }
        public virtual Applications Applications { get; set; }
    }
}
