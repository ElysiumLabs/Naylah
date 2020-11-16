using Naylah.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah
{
    public interface IEntityUpdate<TSource>
    {
        void UpdateFrom(TSource source, EntityUpdateOptions options = null);
    }
}
