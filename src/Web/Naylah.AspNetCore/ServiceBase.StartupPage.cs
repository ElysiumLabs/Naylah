using Microsoft.AspNetCore.Builder;
using Naylah.StartPage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah
{
    public abstract partial class ServiceBase<TOptions>
    {
        protected virtual void ConfigureStartupPage(IApplicationBuilder app)
        {
            app.UseStartPage(Options);
        }
    }
}
