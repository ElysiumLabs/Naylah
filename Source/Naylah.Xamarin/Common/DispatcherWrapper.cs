using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Common
{
    public class DispatcherWrapper : IDispatcherWrapper
    {

        public static IDispatcherWrapper Current() => new DispatcherWrapper();

        internal DispatcherWrapper()
        {
        }

        public async Task DispatchAsync(Action action, int delayms = 0)
        {
            await Task.Delay(delayms);

            Device.BeginInvokeOnMainThread(action);

        }

        public async Task DispatchAsync(Func<Task> func, int delayms = 0)
        {
            await Task.Delay(delayms);
            Device.BeginInvokeOnMainThread(() => { func?.Invoke(); });
        }

        public async Task<T> DispatchAsync<T>(Func<T> func, int delayms = 0)
        {
            await Task.Delay(delayms);

            T val = default(T);

            Device.BeginInvokeOnMainThread(
                () => 
                {

                    val = func != null ? func.Invoke() : default(T) ;
                }
                );

            return val;
        }

        public async void Dispatch(Action action, int delayms = 0)
        {
            await Task.Delay(delayms);
            Device.BeginInvokeOnMainThread(action);
        }

        public T Dispatch<T>(Func<T> func, int delayms = 0) where T : class
        {

            Task.Delay(delayms);

            T val = default(T);

            Device.BeginInvokeOnMainThread(
                () =>
                {

                    val = func != null ? func.Invoke() : default(T);
                }
                );

            return val;

        }

    }
}
