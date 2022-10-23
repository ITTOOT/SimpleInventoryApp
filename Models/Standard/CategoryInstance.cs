using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Models.Standard
{
    //Items can belong to a single catagory or many...
    //...categories can also belong to a category
    public record class CategoryInstance
    {
        // category acts as a “tag” for any entity to the system.
        // allows for many different types of categories
        // categories can be used to add more information to items, such as other categories
    }
}
