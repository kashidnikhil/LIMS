namespace MyTraining1101Demo.LIMS.Library.Tests.Units
{

    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Unit")]
    public class Unit : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Unit Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
