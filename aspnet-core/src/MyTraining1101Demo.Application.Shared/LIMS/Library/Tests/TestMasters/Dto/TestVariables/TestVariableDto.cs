using System;
using System.Collections.Generic;
using System.Text;

namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.Dto.TestVariables
{
    public class TestVariableDto
    {
        public Guid Id { get; set; }

        public string Variable { get; set; }

        public string Description { get; set; }

        public string CellValue { get; set; }

        public Guid TestId { get; set; }
    }
}
