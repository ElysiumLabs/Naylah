using System.Runtime.CompilerServices;

namespace Naylah.Domain.Abstractions
{
    public class Notification : IEvent
    {
        public Notification()
        {
        }

        public Notification(string value, [CallerMemberName]string key = null)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}