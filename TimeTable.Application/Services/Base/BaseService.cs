using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.DataAccess.Contracts;

namespace TimeTable.Application.Services.Base
{
    public abstract class BaseService : IBaseService
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IAppConfig appConfig;

        public BaseService(IUnitOfWork unitOfWork, IAppConfig appConfig)
        {
            this.unitOfWork = unitOfWork;
            this.appConfig = appConfig;
        }
    }
}
