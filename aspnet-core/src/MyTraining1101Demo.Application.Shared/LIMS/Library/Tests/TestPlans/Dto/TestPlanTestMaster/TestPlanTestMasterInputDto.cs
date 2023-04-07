using System;
using System.Collections.Generic;
using System.Text;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestPlans.Dto.TestPlanTestMaster
{
    public class TestPlanTestMasterInputDto
    {
        public Guid? Id { get; set; }

        public string Limit { get; set; }

        public Guid? TestPlanId { get; set; }

        public Guid? TestId { get; set; }

    }
}
