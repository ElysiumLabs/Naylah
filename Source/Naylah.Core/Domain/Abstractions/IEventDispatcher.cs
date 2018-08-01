namespace Naylah.Domain.Abstractions
{
    public interface IEventDispatcher
    {
        void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IEvent;
    }
}