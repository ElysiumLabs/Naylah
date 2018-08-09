using System;

namespace Naylah.App.Navigation
{
    public interface IPageBackHandler
    {
        Func<bool?> HandleBack { get; }
    }
}