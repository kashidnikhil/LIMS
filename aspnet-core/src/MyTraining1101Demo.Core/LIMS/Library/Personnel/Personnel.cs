namespace MyTraining1101Demo.LIMS.Library.Personnel
{

    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Personnel")]
    public class Personnel : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Personnel Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
