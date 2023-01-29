namespace MyTraining1101Demo.LIMS.Library.Personnel.Dto
{
    using Abp.Runtime.Validation;
    using MyTraining1101Demo.Dto;

    public class PersonnelSearchDto : PagedAndSortedInputDto, IShouldNormalize
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
