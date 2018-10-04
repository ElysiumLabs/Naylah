using System;

namespace Naylah
{
    public interface IModifiable
    {
        DateTimeOffset? CreatedAt { get; set; }

        DateTimeOffset? UpdatedAt { get; set; }

        string Version { get; set; }

        bool Deleted { get; set; }
    }
}