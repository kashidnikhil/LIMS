namespace MyTraining1101Demo.LIMS.Library.Customers.Source
{

    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Source")]
    public class Source : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Source Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
