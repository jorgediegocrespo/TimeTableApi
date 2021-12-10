using Polly.Retry;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Mappers;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly ICompanyRepository repository;
        private readonly CompanyMapper mapper;

        public CompanyService(IUnitOfWork unitOfWork, 
                              ICompanyRepository repository, 
                              IAppConfig appConfig)
            : base(appConfig)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            mapper = new CompanyMapper();
        }

        public async Task<Company> GetAsync()
        {
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    CompanyEntity company = await repository.GetAsync();
                    return mapper.MapReading(company);
                });
        }

        public virtual async Task UpdateAsync(Company businessModel)
        {
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            await retryPolity.ExecuteAsync(async () => await UpdateAsync(businessModel, true));
        }

        public virtual async Task UpdateAsync(Company businessModel, bool withTransaction)
        {
            await ValidateEntityToUpdateAsync(businessModel);
            CompanyEntity entity = await MapUpdatingAsync(businessModel);
            await repository.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
        }

        protected virtual async Task ValidateEntityToUpdateAsync(Company businessModel)
        {
            CompanyEntity currentCompany = await repository.GetAsync();
            if (currentCompany.Id != businessModel.Id)
                throw new NotValidItemException(ErrorCodes.ITEM_NOT_EXISTS, $"There is not any company with the id {businessModel.Id}");
        }

        protected virtual Task<CompanyEntity> MapUpdatingAsync(Company businessModel)
        {
            CompanyEntity entity = mapper.MapUpdating(businessModel);
            return Task.FromResult(entity);
        }
    }
}
