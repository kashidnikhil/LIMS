namespace MyTraining1101Demo.LIMS.Library.Personnel
{
    using Abp.Application.Services.Dto;
    using MyTraining1101Demo.LIMS.Library.Personnel.Dto;
    using MyTraining1101Demo.LIMS.Shared;
    using System;
    using System.Threading.Tasks;

    public class PersonnelAppService : MyTraining1101DemoAppServiceBase, IPersonnelAppService
    {
        private readonly IPersonnelManager _personnelManager;

        public PersonnelAppService(
          IPersonnelManager personnelManager
         )
        {
            _personnelManager = personnelManager;
        }


        public async Task<PagedResultDto<PersonnelDto>> GetPersonnels(PersonnelSearchDto input)
        {
            try
            {
                var result = await this._personnelManager.GetPaginatedPersonnelListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<ResponseDto> InsertOrUpdatePersonnel(PersonnelInputDto input)
        {
            try
            {
                var insertedOrUpdatedPersonnelId = await this._personnelManager.InsertOrUpdatePersonnelIntoDB(input);

                return insertedOrUpdatedPersonnelId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeletePersonnel(Guid personnelId)
        {
            try
            {
                var isPersonnelDeleted = await this._personnelManager.DeletePersonnelFromDB(personnelId);
                return isPersonnelDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<PersonnelDto> GetPersonnelById(Guid personnelId)
        {
            try
            {
                var response = await this._personnelManager.GetPersonnelByIdFromDB(personnelId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> RestorePersonnel(Guid personnelId)
        {
            try
            {
                var response = await this._personnelManager.RestorePersonnel(personnelId);
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
