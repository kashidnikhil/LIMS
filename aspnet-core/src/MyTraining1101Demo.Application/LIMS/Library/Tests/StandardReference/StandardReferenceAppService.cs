﻿using Abp.Application.Services.Dto;
using MyTraining1101Demo.LIMS.Library.Personnel.Dto;
using MyTraining1101Demo.LIMS.Library.Personnel;
using System.Threading.Tasks;
using System;
using MyTraining1101Demo.LIMS.Library.Tests.StandardReference.Dto;

namespace MyTraining1101Demo.LIMS.Library.Tests.StandardReference
{
    public class StandardReferenceAppService : MyTraining1101DemoAppServiceBase, IStandardReferenceAppService
    {
        private readonly IStandardReferenceManager _standardReferenceManager;

        public StandardReferenceAppService(
          IStandardReferenceManager standardReferenceManager
         )
        {
            _standardReferenceManager = standardReferenceManager;
        }


        public async Task<PagedResultDto<StandardReferenceDto>> GetStandardReferences(StandardReferenceSearchDto input)
        {
            try
            {
                var result = await this._standardReferenceManager.GetPaginatedStandardReferenceListFromDB(input);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async Task<Guid> InsertOrUpdateStandardReference(StandardReferenceInputDto input)
        {
            try
            {
                var insertedOrUpdatedStandardReferenceId = await this._standardReferenceManager.InsertOrUpdateStandardReferenceIntoDB(input);

                return insertedOrUpdatedStandardReferenceId;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<bool> DeleteStandardReference(Guid personnelId)
        {
            try
            {
                var isStandardReferenceDeleted = await this._standardReferenceManager.DeleteStandardReferenceFromDB(personnelId);
                return isStandardReferenceDeleted;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw ex;
            }

        }

        public async Task<StandardReferenceDto> GetStandardReferenceById(Guid standardReferenceId)
        {
            try
            {
                var response = await this._standardReferenceManager.GetStandardReferenceByIdFromDB(standardReferenceId);
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
