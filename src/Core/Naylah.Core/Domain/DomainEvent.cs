using Naylah.Domain.Abstractions;
using Naylah.Extensions;
using System;
using System.Collections.Generic;

namespace Naylah.Domain
{

#if NETSTANDARD2_0
    [Obsolete("Also note that it would be good to introduce an IEventDispatcher abstraction. Don't call a static class from within your code. Even Udi Dahan (who initially described such static class a long time ago) now considers this an anti-pattern. Instead, inject an IEventDispatcher abstraction into classes that require event dispatching.")]
    public class DomainEvent
    {
        public static IServiceProvider Resolver { get; set; }

        public static void Raise<T>(T domainEvent) where T : IEvent
        {
            var handlers = GetHandlersFor<T>();

            handlers?.ForEach(
                handler =>
                {
                    try
                    {
                        handler?.Handle(domainEvent);
                    }
                    catch (Exception ex)
                    {
                        throw new HandleEventException("Fail to handle event: " + ex.Message, ex);
                    }
                }
                );
        }

        public static void Raise<T>(Action<T> messageCtor) where T : IEvent, new()
        {
            var message = new T();
            messageCtor(message);
            Raise(message);
        }

        private static IEnumerable<dynamic> GetHandlersFor<T>() where T : IEvent
        {
            var handlerType = typeof(IHandler<>);
            var genericHandlerType = handlerType.MakeGenericType(typeof(T));

            var enumerableType = typeof(IEnumerable<>);
            var enumerableGenericHandlerType = enumerableType.MakeGenericType(genericHandlerType);

            return (IEnumerable<dynamic>)Resolver.GetService(enumerableGenericHandlerType);
        }
    }
#endif
}