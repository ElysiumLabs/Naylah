using Naylah.Domain.Abstractions;
using Naylah.Extensions;
using System;
using System.Collections.Generic;

namespace Naylah.Domain
{
    public class Dispatcher : IEventDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Dispatch<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = GetHandlersFor<TEvent>();

            handlers?.ForEach(
                handler =>
                {
                    try
                    {
                        handler?.Handle(@event);
                    }
                    catch (Exception ex)
                    {
                        throw new HandleEventException("Fail to handle event: " + ex.Message, ex);
                    }
                }
                );
        }

        public void Dispatch<TEvent>(Action<TEvent> messageCtor) where TEvent : IEvent, new()
        {
            var message = new TEvent();
            messageCtor(message);
            Dispatch(message);
        }

        public IEnumerable<dynamic> GetHandlersFor<T>() where T : IEvent
        {
            var handlerType = typeof(IHandler<>);
            var genericHandlerType = handlerType.MakeGenericType(typeof(T));

            var enumerableType = typeof(IEnumerable<>);
            var enumerableGenericHandlerType = enumerableType.MakeGenericType(genericHandlerType);

            return (IEnumerable<dynamic>)serviceProvider.GetService(enumerableGenericHandlerType);
        }
    }
}