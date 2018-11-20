using Naylah.Data.Access;
using Naylah.Domain.Abstractions;
using System;

namespace Naylah.Services
{
    public class DataService
    {
        public DataService(IUnitOfWork unitOfWork) : this(unitOfWork, null)
        {
        }

        public DataService(IUnitOfWork unitOfWork, IHandler<Notification> notificationsHandler)
        {
            UnitOfWork = unitOfWork;
            NotificationsHandler = notificationsHandler;

            CanCommit = () => { return true; };
        }

        public Func<bool> CanCommit { get; set; }

        protected IHandler<Notification> NotificationsHandler { get; private set; }

        protected IUnitOfWork UnitOfWork { get; private set; }

        public bool Commit()
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

        public bool CommitDo(Action ifCommitAction)
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