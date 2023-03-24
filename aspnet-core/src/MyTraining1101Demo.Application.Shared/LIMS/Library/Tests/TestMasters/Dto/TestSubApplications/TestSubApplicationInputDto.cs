using System;
using System.Collections.Generic;
using System.Text;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestSubApplications
{
    public class TestSubApplicationInputDto
    {
        public Guid? Id { get; set; }

        public bool IsMOEF { get; set; }
        public bool IsNABL { get; set; }

        public Guid? SubApplicationId { get; set; }

        public Guid? TestId { get; set; }
    }
}
