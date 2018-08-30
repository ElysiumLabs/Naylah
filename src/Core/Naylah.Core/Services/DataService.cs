using Naylah.Data.Access;
using Naylah.Domain;
using Naylah.Domain.Abstractions;
using System;

namespace Naylah.Services
{
    public class DataService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHandler<Notification> notificationsHandler;

        public DataService(IUnitOfWork unitOfWork) : this(unitOfWork, null)
        {
        }

        public DataService(IUnitOfWork unitOfWork, IHandler<Notification> notificationsHandler)
        {
            this.unitOfWork = unitOfWork;
            this.notificationsHandler = notificationsHandler;

            CanCommit = () => { return true; };
        }

        public Func<bool> CanCommit { get; set; }

        public bool Commit()
        {
            if (notificationsHandler.HasEvents())
                return false;

            if (CanCommit?.Invoke() == true)
            {
                unitOfWork.Commit();
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