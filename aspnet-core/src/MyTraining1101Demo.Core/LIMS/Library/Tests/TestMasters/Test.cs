namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters
{
    using Microsoft.EntityFrameworkCore;
    using MyTraining1101Demo.LIMS.Library.Tests.Application;
    using MyTraining1101Demo.LIMS.Library.Tests.Techniques;
    using MyTraining1101Demo.LIMS.Library.Tests.Units;
    using System.ComponentModel;
    using Abp.Domain.Entities.Auditing;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using MyTraining1101Demo.LIMS.Library.Tests.SubApplications;

    [Table("Test")]
    public class Test : FullAuditedEntity<Guid>
    {
        [Required(ErrorMessage = "Enter Test Name")]
        public string Name { get; set; }

        public bool IsDefaultTechnique { get; set; }

        public string Method { get; set; }

        public string MethodDescription { get; set; }

        public string WorksheetName { get; set; }

        public bool IsSC { get; set; }

        [DefaultValue(0.00)]
        [Precision(18, 2)]
        public decimal Rate { get; set; }

        public virtual Guid? ApplicationId { get; set; }
        public virtual Applications Application { get; set; }

        public virtual Guid? UnitId { get; set; }
        public virtual Unit Unit { get; set; }

        public virtual Guid? TechniqueId { get; set; }
        public virtual Technique Technique { get; set; }
    }
}
