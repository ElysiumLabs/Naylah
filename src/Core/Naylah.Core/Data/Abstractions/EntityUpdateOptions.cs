using Naylah.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naylah
{
    public class EntityUpdateOptions : Dictionary<string, object>
    {
        public static EntityUpdateOptions Default = new EntityUpdateOptions(UpsertType.Instance);

        public EntityUpdateOptions(UpsertType upsertType = UpsertType.Instance, params KeyValuePair<string, object>[] options)
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
