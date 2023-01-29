namespace MyTraining1101Demo.LIMS.Library.Tests.Application
{
    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Applications")]
    public class Applications: FullAuditedEntity<Guid>
    {

        [Required(ErrorMessage = "Enter Application Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
