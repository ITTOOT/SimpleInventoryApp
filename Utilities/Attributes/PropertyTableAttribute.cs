using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Utilities.Attributes
{
    //Decorate each of the models with these attributes based upon the CREATED tables
    //TableAttribute (used elsewhare) tells Dapper.Contrib which tables on which to perform CRUD methods
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class PropertyTableAttribute : Attribute // Custom attribute
    {
        //This PropertyTableAttribute is used to define tables not decorated by [TableAttribute] for Categories and Custom Attributes
        public PropertyTableAttribute(string propertyName, string name)
        {
            PropertyName = propertyName;
            Name = name;
        }

        public string PropertyName { get; set; }
        public string Name { get; set; }
    }
}
