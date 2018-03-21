using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Xamarin.Events
{
    public class CheckedEventArgs : EventArgs
    {
        public CheckedEventArgs(bool value)
        {
            Value = value;
        }

        public bool Value { get; private set; }
    }
}