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
    public abstract class BaseService<T, R> : IBaseCrudService<T>
        where T : IBaseBusinessModel
        where R : class, IBaseWithIdEntity
    {
        protected readonly IRepository<R> repository;
        protected readonly IAppConfig appConfig;
        protected readonly IMapper<T, R> mapper;

        public BaseService(IRepository<R> repository, IAppConfig appConfig, IMapper<T, R> mapper)
        {
            this.repository = repository;
            this.appConfig = appConfig;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            var retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    IEnumerable<R> allEntities = await repository.GetAllAsync();
                    return allEntities.Select(o => mapper.Map(o));
                });
        }

        public async Task<T> GetAsync(int id)
        {
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            var retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    R entity = await repository.GetAsync(id);
                    return mapper.Map(entity);
                });
        }

        public async Task<T> AddAsync(T entity)
        {
            await ValidateEntityToAddAsync(entity);
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            AsyncRetryPolicy retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    R addedEntity = await repository.AddAsync(mapper.Map(entity));
                    return mapper.Map(addedEntity);
                });
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await ValidateEntityToUpdateAsync(entity);
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            AsyncRetryPolicy retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    var updatedEntity = await repository.UpdateAsync(mapper.Map(entity));
                    return mapper.Map(updatedEntity);
                });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await ValidateEntityToDeleteAsync(id);
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);

            AsyncRetryPolicy retryPolity = Policy.Handle<Exception>().WaitAndRetryAsync(maxTrys, i => timeToWait);
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    await repository.DeleteAsync(id);
                    return true;
                });
        }

        protected virtual Task ValidateEntityToAddAsync(T entity)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task ValidateEntityToUpdateAsync(T entity)
        {
            bool existsItem = await repository.ExistsAsync(x => x.Id == entity.Id);
            if (!existsItem)
                throw new NotValidItemException(ErrorCodes.ITEM_NOT_EXISTS, $"There is not any item with the id {entity.Id}");
        }

        protected virtual async Task ValidateEntityToDeleteAsync(int id)
        {
            bool existsItem = await repository.ExistsAsync(x => x.Id == id);
            if (!existsItem)
                throw new NotValidItemException(ErrorCodes.ITEM_NOT_EXISTS, $"There is not any item with the id {id}");
        }
    }
}
