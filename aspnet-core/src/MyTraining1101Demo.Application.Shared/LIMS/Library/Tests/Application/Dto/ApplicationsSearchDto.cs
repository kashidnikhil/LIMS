namespace MyTraining1101Demo.LIMS.Library.Tests.Application.Dto
{
    using Abp.Runtime.Validation;
    using MyTraining1101Demo.Common;
    using MyTraining1101Demo.Dto;
    public class ApplicationsSearchDto : PagedAndSortedInputDto, IShouldNormalize
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
