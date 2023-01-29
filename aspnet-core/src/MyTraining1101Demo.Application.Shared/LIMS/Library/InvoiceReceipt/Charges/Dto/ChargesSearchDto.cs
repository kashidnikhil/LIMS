namespace MyTraining1101Demo.LIMS.Library.InvoiceReceipt.Charges.Dto
{
    using Abp.Runtime.Validation;
    using MyTraining1101Demo.Dto;

    public class ChargesSearchDto : PagedAndSortedInputDto, IShouldNormalize
    {
        public string SearchString { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "name";
            }
        }
    }
}
