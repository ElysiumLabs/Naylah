using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah.Data.Abstractions
{
    public interface IEntityUpdate<TSource>
    {
        void UpdateFrom(TSource source, EntityUpdateOptions options = null);
    }
}
