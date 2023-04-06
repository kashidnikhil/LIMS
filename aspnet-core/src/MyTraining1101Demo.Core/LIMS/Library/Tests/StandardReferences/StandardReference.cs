namespace MyTraining1101Demo.LIMS.Library.Tests.StandardReferences
{
    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("StandardReference")]
    public class StandardReference : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Standard Reference Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
