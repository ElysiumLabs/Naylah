using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public static class TableDataServiceExtensions
    {
        public static TType GetImplementation<TType>(object obj) where TType : class
        {
            var o = obj as TType;
            return o ?? throw new Exception("Does not implement " + typeof(TType).Name);
        }
    }

    
}
