using Naylah.Data.Access;
using Naylah.Domain.Abstractions;
using System;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public class DataServiceBase
    {
        public DataServiceBase(IUnitOfWork unitOfWork) : this(unitOfWork, null)
        {
        }

        public DataServiceBase(IUnitOfWork unitOfWork, IHandler<Notification> notificationsHandler)
        {
            UnitOfWork = unitOfWork;
            NotificationsHandler = notificationsHandler;

            CanCommit = () => { return true; };
        }

        protected Func<bool> CanCommit { get; set; }

        protected IHandler<Notification> NotificationsHandler { get; private set; }

        protected IUnitOfWork UnitOfWork { get; private set; }

        protected internal async Task<bool> CommitAsync()
        {
            if (NotificationsHandler?.HasEvents() == true)
                return false;

            if (CanCommit?.Invoke() == true)
            {
                await UnitOfWork.CommitAsync();
                return true;
            }

            return false;
        }

        protected async Task<bool> CommitDoAsync(Action ifCommitAction)
        {
            var commited = await CommitAsync();

            if (commited)
            {
                ifCommitAction?.Invoke();
            }

            return commited;
        }
    }
}