namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplications.Dto
{
    using System;

    public class SubApplicationDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public Guid ApplicationId { get; set; }
        public string Description { get; set; }
    }
}
