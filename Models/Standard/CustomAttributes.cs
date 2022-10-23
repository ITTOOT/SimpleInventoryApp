using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Models.Standard
{
    public class CustomAttributes : Dictionary<string, string>
    {
        //Inherits from base indicated at above class
        public CustomAttributes() : base() { }
        public CustomAttributes(IDictionary<string, string> values) : base(values) { }
    }

    //Helpers and overrides not implimented yet.
}
