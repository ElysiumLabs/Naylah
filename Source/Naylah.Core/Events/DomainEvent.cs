using Naylah.Core.Events.Contracts;
using Naylah.Core.Helpers.Contracts;

namespace Naylah.Core.Events
{
    public static class DomainEvent
    {
        public static IContainer Container { get; set; }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            try
            {
                if (Container != null)
                {
                    var objs = Container.GetServices(typeof(IHandler<T>));

                    if (objs != null)
                    {
                        foreach (var obj in objs)
                        {
                            ((IHandler<T>)obj).Handle(args);
                        }
                    }
                }
            }
            catch
            {
                //throw;
            }
        }
    }
}