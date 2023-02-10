namespace MyTraining1101Demo.LIMS.Library.Personnel
{
    using Abp.Application.Services.Dto;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.Linq.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MyTraining1101Demo.Configuration;
    using MyTraining1101Demo.LIMS.Library.Personnel.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    public class PersonnelManager : MyTraining1101DemoDomainServiceBase, IPersonnelManager
    {
        private readonly IRepository<Personnel, Guid> _personnelRepository;
        private readonly IConfigurationRoot _appConfiguration;


        public PersonnelManager(
           IRepository<Personnel, Guid> personnelRepository,
           IAppConfigurationAccessor configurationAccessor
            )
        {
            _personnelRepository = personnelRepository;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<PersonnelDto>> GetPaginatedPersonnelListFromDB(PersonnelSearchDto input)
        {
            try
            {
                var taxQuery = this._personnelRepository.GetAll()
                    .Where(x => !x.IsDeleted)
                    .WhereIf(!input.SearchString.IsNullOrWhiteSpace(), item => item.Name.ToLower().Contains(input.SearchString.ToLower()));

                var totalCount = await taxQuery.CountAsync();
                var items = await taxQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultDto<PersonnelDto>(
                totalCount,
                ObjectMapper.Map<List<PersonnelDto>>(items));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        [UnitOfWork]
        public async Task<Guid> InsertOrUpdatePersonnelIntoDB(PersonnelInputDto input)
        {
            try
            {
                var mappedPersonnelItem = ObjectMapper.Map<Personnel>(input);
                var personnelId = await this._personnelRepository.InsertOrUpdateAndGetIdAsync(mappedPersonnelItem);
                await CurrentUnitOfWork.SaveChangesAsync();
                return personnelId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        [UnitOfWork]
        public async Task<bool> DeletePersonnelFromDB(Guid personnelId)
        {
            try
            {
                var personnelItem = await this._personnelRepository.GetAsync(personnelId);

                await this._personnelRepository.DeleteAsync(personnelItem);

                await CurrentUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<PersonnelDto> GetPersonnelByIdFromDB(Guid personnelId)
        {
            try
            {
                var personnelItem = await this._personnelRepository.GetAsync(personnelId);

                return ObjectMapper.Map<PersonnelDto>(personnelItem);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}
