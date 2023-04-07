namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanTestMaster
{
    using System;
    public class TestPlanTestMasterDto
    {
        public Guid Id { get; set; }

        public string Limit { get; set; }

        public Guid TestPlanId { get; set; }

        public Guid TestId { get; set; }
    }
}
