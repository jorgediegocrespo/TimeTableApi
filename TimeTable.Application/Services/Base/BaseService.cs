using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Mappers.Base;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Application.Exceptions;
using TimeTable.Business.Models.Base;
using TimeTable.DataAccess.Contracts.Entities.Base;
using TimeTable.DataAccess.Contracts.Repositories.Base;

namespace TimeTable.Application.Services.Base
{
    public abstract class BaseService<BR, DR, C, U, E> : IBaseCrudService<BR, DR, C, U>
        where BR : IBasicReadingBusinessModel
        where DR : IDetailedReadingBusinessModel
        where C : ICreationBusinessModel
        where U : IUpdatingBusinessModel
        where E : IBaseWithIdEntity
    {
        protected readonly IRepository<E> repository;
        protected readonly IAppConfig appConfig;
        protected readonly IMapper<BR, DR, C, U, E> mapper;

        public BaseService(IRepository<E> repository, IAppConfig appConfig, IMapper<BR, DR, C, U, E> mapper)
        {
            this.repository = repository;
            this.appConfig = appConfig;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BR>> GetAllAsync()
        {
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            var retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    IEnumerable<E> allEntities = await repository.GetAllAsync();
                    return allEntities.Select(o => mapper.MapBasicReading(o));
                });
        }

        public async Task<DR> GetAsync(int id)
        {
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            var retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    E entity = await repository.GetAsync(id);
                    return mapper.MapDetailedReading(entity);
                });
        }

        public async Task<int> AddAsync(C businessModel)
        {
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            //TODO Review exception managed with polly
            AsyncRetryPolicy retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            return await retryPolity.ExecuteAsync(async () =>
            {
                await ValidateEntityToAddAsync(businessModel);
                return await DoAddOperationsAsync(businessModel);
            });
        }

        protected virtual async Task<int> DoAddOperationsAsync(C businessModel)
        {
            E addedEntity = await repository.AddAsync(mapper.MapCreating(businessModel));
            return addedEntity.Id;
        }

        public async Task UpdateAsync(U businessModel)
        {
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            AsyncRetryPolicy retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            await retryPolity.ExecuteAsync(
                async () =>
                {
                    await ValidateEntityToUpdateAsync(businessModel);
                    await DoUpdateOperationsAsync(businessModel);
                });
        }

        protected virtual async Task DoUpdateOperationsAsync(U businessModel)
        {
            await repository.UpdateAsync(mapper.MapUpdating(businessModel));
        }

        public async Task DeleteAsync(int id)
        {
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            AsyncRetryPolicy retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            await retryPolity.ExecuteAsync(
                async () =>
                {
                    await ValidateEntityToDeleteAsync(id);
                    await DoDeleteOperationsAsync(id);
                });
        }

        protected virtual async Task DoDeleteOperationsAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        protected virtual Task ValidateEntityToAddAsync(C businessModel)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task ValidateEntityToUpdateAsync(U businessModel)
        {
            bool existsItem = await repository.ExistsAsync(x => x.Id == businessModel.Id);
            if (!existsItem)
                throw new NotValidItemException(ErrorCodes.ITEM_NOT_EXISTS, $"There is not any item with the id {businessModel.Id}");
        }

        protected virtual async Task ValidateEntityToDeleteAsync(int id)
        {
            bool existsItem = await repository.ExistsAsync(x => x.Id == id);
            if (!existsItem)
                throw new NotValidItemException(ErrorCodes.ITEM_NOT_EXISTS, $"There is not any item with the id {id}");
        }
    }
}
