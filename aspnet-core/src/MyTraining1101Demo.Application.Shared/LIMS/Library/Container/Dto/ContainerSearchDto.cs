namespace MyTraining1101Demo.LIMS.Library.Container.Dto
{
    using Abp.Runtime.Validation;
    using MyTraining1101Demo.Dto;

    public class ContainerSearchDto : PagedAndSortedInputDto , IShouldNormalize
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
