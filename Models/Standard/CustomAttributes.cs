using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Models.Standard
{
    public class CustomAttributes : Dictionary<string, string>
    {
        public CustomAttributes() : base() { }
        public CustomAttributes(IDictionary<string, string> values) : base(values) { }
    }

    // IEquatable and Equals / GetHasCode()
}
