namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestMaster
{
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto.CustomerPOs;
    using MyTraining1101Demo.LIMS.Library.Customers.CustomerMaster.Dto;
    using System;
    using System.Collections.Generic;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestSubApplications;
    using MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestVariables;

    public class TestMasterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsDefaultTechnique { get; set; }

        public string Method { get; set; }

        public string MethodDescription { get; set; }

        public string WorksheetName { get; set; }

        public bool IsSC { get; set; }

        public decimal Rate { get; set; }

        public IList<TestSubApplicationDto> TestSubApplications { get; set; }

        public IList<TestVariableDto> TestVariables { get; set; }

        public virtual Guid? ApplicationId { get; set; }

        public virtual Guid? UnitId { get; set; }

        public virtual Guid? TechniqueId { get; set; }
    }
}
