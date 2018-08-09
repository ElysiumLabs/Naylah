using Naylah.DI.Abstractions;
using Naylah.Domain.Abstractions;
using Naylah.Extensions;
using System;
using System.Collections.Generic;

namespace Naylah.Domain
{
    public class DomainEvent
    {
        //public static IEventDispatcher Dispatcher { get; set; }

        //public static void Raise<T>(T @event) where T : IDomainEvent
        //{
        //    Dispatcher.Dispatch(@event);
        //}

        public static IDependencyResolver Resolver { get; set; }

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
            return Resolver.GetServices(genericHandlerType);
        }
    }
}