using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Models.Standard
{
    // To ,be inventoried and tracked with "tags" associated with custom attributes
    public record class ProductInstance
    {
        // 
        public StringCollection ProductImageUris { get; set; } = new StringCollection();
        public StringCollection ValidSkus { get; set; } = new StringCollection();
    }
}
