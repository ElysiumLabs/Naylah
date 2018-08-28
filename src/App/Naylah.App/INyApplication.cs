using System;

namespace Naylah.App
{
    public interface INyApplication
    {
        IServiceProvider Services { get; set; }
    }
}