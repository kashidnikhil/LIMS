namespace MyTraining1101Demo.LIMS.Library.Container
{
    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Container")]
    public class Container : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Container Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }

}
