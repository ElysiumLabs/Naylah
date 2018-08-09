using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Naylah.App.UI.Style
{
    public class StyleKitBase : Dictionary<string, object>
    {
        public StyleKitBase()
        {
        }

        public T GetIfExistsValue<T>([CallerMemberName]string key = null)
        {
            try
            {
                //if (this.ContainsKey(key))
                {
                    return (T)this[key];
                }
            }
            catch (System.Exception)
            {
                return default(T);
            }
        }

        public void SetValue<T>(T value, [CallerMemberName]string key = null)
        {
            this[key] = value;
        }
    }
}