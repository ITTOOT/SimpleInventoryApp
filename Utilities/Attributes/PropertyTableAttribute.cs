using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Utilities.Attributes
{
    // Notify repository service tables, using attributes
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class PropertyTableAttribute : Attribute // Custom attribute
    {
        // Used to define tables not decorated by [TableAttribute] for Categories and Custom Attributes
        public PropertyTableAttribute(string propertyName, string name)
        {
            PropertyName = propertyName;
            Name = name;
        }

        public string PropertyName { get; set; }
        public string Name { get; set; }
    }
}
