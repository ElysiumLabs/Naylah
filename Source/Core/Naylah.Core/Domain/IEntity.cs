using Naylah.Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Naylah.Domain
{
    public interface IEntity<T> : IEntity, IModifiableEntity
    {
        T Id { get; set; }
    }

    public interface IEntity
    {
        ICollection<Notification> Notifications { get; }
    }

    public interface IModifiableEntity
    {
        DateTimeOffset? CreatedAt { get; set; }

        DateTimeOffset? UpdatedAt { get; set; }

        string Version { get; set; }

        bool Deleted { get; set; }
    }
}