namespace MyTraining1101Demo.LIMS.Library.Tests.Technique
{
    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Technique")]
    public class Technique : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Technique Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
