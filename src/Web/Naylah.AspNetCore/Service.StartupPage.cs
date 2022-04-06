using Microsoft.AspNetCore.Builder;
using Naylah.StartPage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah
{
    public abstract partial class Service<TOptions>
    {
        public virtual void ConfigureStartupPage(IApplicationBuilder app)
        {
            app.UseStartPage(Options);
        }
    }
}
