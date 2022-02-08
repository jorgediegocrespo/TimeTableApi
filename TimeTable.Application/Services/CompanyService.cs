using System;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Exceptions;
using TimeTable.Application.Services.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Services
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository repository;

        public CompanyService(IUnitOfWork unitOfWork, 
                              ICompanyRepository repository, 
                              IAppConfig appConfig)
            : base(unitOfWork, appConfig)
        {
            this.repository = repository;
        }

        public async Task<Company> GetAsync()
        {
            CompanyEntity entity = await repository.GetAsync();
            return MapReading(entity);
        }

        public async Task UpdateAsync(Company businessModel)
        {
            CompanyEntity entity = await repository.GetAsync();
            ValidateEntityToUpdate(entity, businessModel);

            CompanyEntity entityToUpdate = await repository.AttachAsync(businessModel.Id, businessModel.RowVersion);
            MapUpdating(entityToUpdate, businessModel);
            await unitOfWork.SaveChangesAsync();
        }

        private Company MapReading(CompanyEntity entity)
        {
            return new Company()
            {
                Id = entity.Id,
                Name = entity.Name,
                RowVersion = entity.RowVersion,
            };
        }

        private void MapUpdating(CompanyEntity entity, Company businessModel)
        {
            entity.Name = businessModel.Name;
        }

        private void ValidateEntityToUpdate(CompanyEntity entity, Company businessModel)
        {
            if (entity.Id != businessModel.Id)
                throw new NotValidOperationException(ErrorCodes.ITEM_NOT_EXISTS, $"There is not any company with the id {businessModel.Id}");
        }
    }
}
