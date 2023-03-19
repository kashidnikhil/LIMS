using System;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestMaster
{
    public class TestMasterInputDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public bool IsDefaultTechnique { get; set; }

        public string Method { get; set; }

        public string MethodDescription { get; set; }

        public string WorksheetName { get; set; }

        public bool IsSC { get; set; }

        public decimal Rate { get; set; }

        public virtual Guid? ApplicationId { get; set; }

        public virtual Guid? UnitId { get; set; }

        public virtual Guid? TechniqueId { get; set; }
    }
}
