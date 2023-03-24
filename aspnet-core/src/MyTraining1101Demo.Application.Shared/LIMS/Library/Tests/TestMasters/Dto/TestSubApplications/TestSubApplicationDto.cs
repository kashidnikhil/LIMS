using System;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestSubApplications
{
    public class TestSubApplicationDto
    {
        public Guid Id { get; set; }
        public bool IsMOEF { get; set; }
        public bool IsNABL { get; set; }

        public Guid TestId { get; set; }
        
        public Guid SubApplicationId { get; set; }
        public string Name { get; set; }
    }
}
