namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestMaster
{
    using System;

    public class TestMasterListDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public decimal Rate { get; set; }

        public string Method { get; set; }

        public string UnitName { get; set; }

        public string ApplicationName { get; set; }
        public string TechniqueName { get; set; }
    }
}
