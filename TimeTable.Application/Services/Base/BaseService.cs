using Polly;
using Polly.Retry;
using System;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Application.Exceptions;
using TimeTable.DataAccess.Contracts;

namespace TimeTable.Application.Services.Base
{
    public abstract class BaseService : IBaseService
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IAppConfig appConfig;

        public BaseService(IUnitOfWork unitOfWork,
                           IAppConfig appConfig)
        {
            this.unitOfWork = unitOfWork;
            this.appConfig = appConfig;
        }

        protected AsyncRetryPolicy GetRetryPolicy()
        {
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);
            return Policy
                .Handle<Exception>(ex => !(ex is NotValidOperationException) && !(ex is ForbidenActionException))
                .WaitAndRetryAsync(maxTrys, i => timeToWait);
        }
    }
}
