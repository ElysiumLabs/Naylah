using Naylah.Data.Access;
using Naylah.Domain.Abstractions;
using System;

namespace Naylah.Data.Services
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

        protected bool Commit()
        {
            if (NotificationsHandler?.HasEvents() == true)
                return false;

            if (CanCommit?.Invoke() == true)
            {
                UnitOfWork.Commit();
                return true;
            }

            return false;
        }

        protected bool CommitDo(Action ifCommitAction)
        {
            var commited = Commit();

            if (commited)
            {
                ifCommitAction?.Invoke();
            }

            return commited;
        }
    }
}