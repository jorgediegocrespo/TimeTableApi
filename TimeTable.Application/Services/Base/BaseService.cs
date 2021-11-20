using Polly;
using Polly.Retry;
using System;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Application.Exceptions;

namespace TimeTable.Application.Services.Base
{
    public abstract class BaseService : IBaseService
    {
        protected readonly IAppConfig appConfig;

        public BaseService(IAppConfig appConfig)
        {
            this.appConfig = appConfig;
        }

        protected AsyncRetryPolicy GetRetryPolicy()
        {
            int maxTrys = appConfig.MaxTrys;
            TimeSpan timeToWait = TimeSpan.FromSeconds(appConfig.SecondToWait);
            return Policy
                .Handle<Exception>(ex => !(ex is NotValidItemException) && !(ex is ForbidenActionException))
                .WaitAndRetryAsync(maxTrys, i => timeToWait);
        }
    }
}
