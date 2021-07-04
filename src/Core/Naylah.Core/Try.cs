using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah
{
    public class Try
    {
        public static T Get<T>(Func<T> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (Exception)
            {

            }
            return default;
        }

        public static void Run(Action func)
        {
            try
            {
                func.Invoke();
            }
            catch (Exception)
            {
            }
        }
    }
}
