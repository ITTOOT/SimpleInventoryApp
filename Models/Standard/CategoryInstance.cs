using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Models.Standard
{
    // To ,be inventoried and tracked with "tags" associated with custom attributes
    public record class CategoryInstance
    {
        // category acts as a “tag” for any entity to the system.
        // allows for many different types of categories
        // categories can be used to add more information to items, such as other categories
    }
}
