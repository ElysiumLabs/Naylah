using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Xamarin.Models
{
    public class InfoDetail
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }


        public Action<object> Action { get; set; }



    };
}
