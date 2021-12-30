using Polly.Retry;
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
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    CompanyEntity company = await repository.GetAsync();
                    return new Company()
                    {
                        Id = company.Id,
                        Name = company.Name,
                    };
                });
        }

        public async Task UpdateAsync(Company businessModel)
        {
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            await retryPolity.ExecuteAsync(async () =>
            {
                CompanyEntity entity = await repository.GetAsync();
                ValidateEntityToUpdate(entity, businessModel);
                MapUpdating(entity, businessModel);

                await repository.UpdateAsync(entity);
                await unitOfWork.SaveChangesAsync();
            });
        }

        private void ValidateEntityToUpdate(CompanyEntity entity, Company businessModel)
        {
            if (entity.Id != businessModel.Id)
                throw new NotValidOperationException(ErrorCodes.ITEM_NOT_EXISTS, $"There is not any company with the id {businessModel.Id}");
        }

        private void MapUpdating(CompanyEntity entity, Company businessModel)
        {
            entity.Name = businessModel.Name;
        }
    }
}
