using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Models.Standard
{
    public class CategoryCollection : List<CategoryCollectionEntry>
    {
        public CategoryCollection() : base() { }

        public CategoryCollection(IEnumerable<CategoryCollectionEntry> entries) : base(entries) { }

        public CategoryCollection(IEnumerable<int> catogoryIds) : 
            base(catogoryIds.Select(i => new CategoryCollectionEntry { CategoryId = i }).ToArray()) { }

        public CategoryCollection(params int[] catogoryIds) :
            base(catogoryIds.Select(i => new CategoryCollectionEntry { CategoryId = i }).ToArray()) { }
    }

    public class CategoryCollectionEntry
    {
        public string Category { get; set; }
        public int CategoryId { get; set; }
    }

    // IEquatable and Equals / GetHasCode()
}
