namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplications
{
    using Abp.Domain.Entities.Auditing;
    using MyTraining1101Demo.LIMS.Library.Tests.Application;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SubApplications")]
    public class SubApplication : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter SubApplication Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Guid? ApplicationId { get; set; }
        public virtual Applications Application { get; set; }
    }
}
