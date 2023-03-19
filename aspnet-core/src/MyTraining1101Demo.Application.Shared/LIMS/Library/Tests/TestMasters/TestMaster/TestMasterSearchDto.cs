namespace MyTraining1101Demo.LIMS.Library.Tests.TestMasters.TestMaster
{
    using Abp.Runtime.Validation;
    using MyTraining1101Demo.Dto;
    public class TestMasterSearchDto : PagedAndSortedInputDto, IShouldNormalize
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
