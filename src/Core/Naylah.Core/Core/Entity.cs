using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Naylah
{
    public abstract class Entity : IEntity<string>, IModifiable, INotificable
    {
        public Entity() : this(false)
        {
        }

        public Entity(bool generateId)
        {
            if (generateId)
            {
                this.GenerateId();
            }

            Notifications = new Collection<Notification>();
        }

        public string Id { get; set; }

        public ICollection<Notification> Notifications { get; internal set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public string Version { get; set; }

        public bool Deleted { get; set; }

        #region IComparable

        private int? _requestedHashCode;

        public bool IsTransient()
        {
            return this.Id == default(string);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        #endregion IComparable

        public static T Create<T>() where T : class, IEntity, new()
        {
            return Activator.CreateInstance<T>();
        }
    }
}