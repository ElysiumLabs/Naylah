using System;
using System.Collections.Generic;
using System.Text;

namespace Naylah
{
    public class ServiceOptions
    {
        public string Name { get; set; }

        public string Organization { get; set; }

        public bool UseDefaultStartupPage { get; set; } = true;


        public static TOptions CreateDefault<TOptions>(string name)
            where TOptions : ServiceOptions, new()
        {
            return new TOptions()
            {
                Name = name,
            };
        }
    }
}
