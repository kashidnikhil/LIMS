

namespace MyTraining1101Demo.LIMS.Library.Tests.SubApplications.Dto
{
    using Abp.Runtime.Validation;
    using MyTraining1101Demo.Dto;

    public class SubApplicationSearchDto : PagedAndSortedInputDto, IShouldNormalize
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
