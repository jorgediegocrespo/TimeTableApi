using Polly.Retry;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Mappers.Base;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Application.Exceptions;
using TimeTable.Business.Models.Base;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities.Base;
using TimeTable.DataAccess.Contracts.Repositories.Base;

namespace TimeTable.Application.Services.Base
{
    public abstract class BaseCrudService<BR, DR, C, U, E> : BaseService, IBaseCrudService<BR, DR, C, U>
        where BR : IBasicReadingBusinessModel
        where DR : IDetailedReadingBusinessModel
        where C : ICreationBusinessModel
        where U : IUpdatingBusinessModel
        where E : IBaseWithIdEntity
    {
        protected readonly ICrudRepository<E> repository;
        protected readonly IMapper<BR, DR, C, U, E> mapper;

        public BaseCrudService(IUnitOfWork unitOfWork, 
                               ICrudRepository<E> repository, 
                               IAppConfig appConfig, 
                               IMapper<BR, DR, C, U, E> mapper) 
            : base(unitOfWork, appConfig)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public virtual async Task<IEnumerable<BR>> GetAllAsync()
        {
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    await ValidateToGetAllAsync();
                    IEnumerable<E> allEntities = await repository.GetAllAsync();
                    return allEntities.Select(o => mapper.MapBasicReading(o));
                });
        }
        
        public virtual async Task<DR> GetAsync(int id)
        {
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            return await retryPolity.ExecuteAsync(
                async () =>
                {
                    await ValidateToGetAsync(id);
                    E entity = await repository.GetAsync(id);
                    return mapper.MapDetailedReading(entity);
                });
        }

        public virtual async Task<int> AddAsync(C businessModel)
        {
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            return await retryPolity.ExecuteAsync(async () => await AddAsync(businessModel, true));
        }

        public virtual async Task<int> AddAsync(C businessModel, bool withTransaction = true)
        {
            await ValidateEntityToAddAsync(businessModel);
            var entity = await MapCreatingAsync(businessModel);
            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return entity.Id;
        }    

        public virtual async Task UpdateAsync(U businessModel)
        {
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            await retryPolity.ExecuteAsync(async () => await UpdateAsync(businessModel, true));
        }

        public virtual async Task UpdateAsync(U businessModel, bool withTransaction)
        {
            E entity = await repository.GetAsync(businessModel.Id);
            await ValidateEntityToUpdateAsync(entity, businessModel);
            await MapUpdatingAsync(entity, businessModel);
            await repository.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            AsyncRetryPolicy retryPolity = GetRetryPolicy();
            await retryPolity.ExecuteAsync(async () => await DeleteAsync(id, true));
        }

        public virtual async Task DeleteAsync(int id, bool withTransaction)
        {
            await ValidateEntityToDeleteAsync(id);
            await repository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        protected virtual Task ValidateToGetAllAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task ValidateToGetAsync(int id)
        {
            return Task.CompletedTask;
        }

        protected virtual Task ValidateEntityToAddAsync(C businessModel)
        {
            return Task.CompletedTask;
        }

        protected virtual Task ValidateEntityToUpdateAsync(E entity, U businessModel)
        {
            if (entity == null)
                throw new NotValidOperationException(ErrorCodes.ITEM_NOT_EXISTS, $"There is not any item with the id {businessModel.Id}");

            return Task.CompletedTask;
        }

        protected virtual async Task ValidateEntityToDeleteAsync(int id)
        {
            bool existsItem = await repository.ExistsAsync(x => x.Id == id);
            if (!existsItem)
                throw new NotValidOperationException(ErrorCodes.ITEM_NOT_EXISTS, $"There is not any item with the id {id}");
        }

        protected virtual Task<E> MapCreatingAsync(C businessModel)
        {
            E entity = mapper.MapCreating(businessModel);
            return Task.FromResult(entity);
        }

        protected virtual Task MapUpdatingAsync(E entity, U businessModel)
        {
            mapper.MapUpdating(entity, businessModel);
            return Task.CompletedTask;
        }
    }
}
