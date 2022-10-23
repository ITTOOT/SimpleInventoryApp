using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Models.Base
{
    public class StringCollection : List<string>
    {

        public StringCollection() : base() { }
        public StringCollection(IEnumerable<string> items) : base(items) { }
        public StringCollection(params string[] items) : base(items) { }
    }
}
