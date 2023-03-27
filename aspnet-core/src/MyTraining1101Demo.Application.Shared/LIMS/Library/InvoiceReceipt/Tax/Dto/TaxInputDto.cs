using System;

namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax.Dto
{
    public class TaxInputDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
    }
}
