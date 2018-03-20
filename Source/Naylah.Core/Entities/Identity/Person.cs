using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Core.Entities.Identity
{
    public class Person : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string FullName { get { return GetFullName(); } }

        

        public string ImageUri { get; set; }



        public string GetFullName()
        {
            return FirstName + " " + LastName;
            //TODO: Culture... Example: Breno Santos, Smith John
        }


    }
}
