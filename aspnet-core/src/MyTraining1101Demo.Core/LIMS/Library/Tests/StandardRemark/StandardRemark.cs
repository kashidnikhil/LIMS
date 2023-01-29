namespace MyTraining1101Demo.LIMS.Library.Tests.StandardRemark
{
    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("StandardRemark")]
    public class StandardRemark : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Standard Remark Name")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
