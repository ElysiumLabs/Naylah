using System;

namespace Naylah.Core.Events.Contracts
{
    public class DomainNotification : IDomainEvent
    {
        public DomainNotification(string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.DateOccurred = DateTime.UtcNow;
        }

        public string Key { get; private set; }
        public string Value { get; private set; }
        public DateTime DateOccurred { get; private set; }
    }
}