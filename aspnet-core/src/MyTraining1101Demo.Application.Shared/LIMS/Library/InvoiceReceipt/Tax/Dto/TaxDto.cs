namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Tax.Dto
{
    using System;

    public class TaxDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public decimal Percentage { get; set; }
    }
}
