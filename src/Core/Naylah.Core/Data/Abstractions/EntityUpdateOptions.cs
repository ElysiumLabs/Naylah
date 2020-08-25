using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naylah.Data.Abstractions
{
    public class EntityUpdateOptions : Dictionary<string, object>
    {
        public static EntityUpdateOptions Default = new EntityUpdateOptions(UpsertType.Update);

        public EntityUpdateOptions(UpsertType upsertType, params KeyValuePair<string, object>[] options)
        {
            UpsertType = upsertType;

            if (options != null)
            {
                foreach (var option in options)
                {
                    Add(option.Key, option.Value);
                }
            }
        }

        public UpsertType UpsertType { get; }
    }
}
