using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInventoryApp.Models.Base;

namespace SimpleInventoryApp.Models.Standard
{
    //An item that can be inventoried and tracked with "tags" associated with custom attributes
    public record class ProductInstance
    {
        //Specific property types - As collections of strings
        public StringCollection ProductImageUris { get; set; } = new StringCollection();
        public StringCollection ValidSkus { get; set; } = new StringCollection();
    }
}
