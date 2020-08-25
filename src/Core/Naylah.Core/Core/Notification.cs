using Naylah.Domain.Abstractions;
using System;
using System.Runtime.CompilerServices;

namespace Naylah
{
    public class Notification : IEvent
    {
        public static Notification FromType(Type type, string value)
        {
            return new Notification() { Key = type.Name, Value = value };
        }

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