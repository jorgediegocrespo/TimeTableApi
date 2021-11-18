using Polly.Retry;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Mappers;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Exceptions;
using TimeTable.Application.Services.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Services
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository repository;
        private readonly CompanyMapper mapper;

        public CompanyService(ICompanyRepository repository, 
                              IAppConfig appConfig)
            : base(appConfig)
        {
            this.repository = repository;
            mapper = new CompanyMapper();
        }

        public async Task<BasicReadingCompany> GetAsync()
        {
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    CompanyEntity company = await repository.GetAsync();
                    if (company == null)
                        throw new NotValidItemException(ErrorCodes.NO_COMPANY_CREATED, $"There is not any company");

                    return mapper.MapBasicReading(company);
                });
        }
    }
}
