using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleInventoryApp.Models.Base;

namespace SimpleInventoryApp.Utilities.SQL.Readers
{
    // Takes the results from a Get or Find query and create concrete instances.
    internal class InstanceReader<Instance> where TInstance : Instance, new()
    {
        public async Task<IEnumerable<TInstance>> ReadInstances(SqlMapper.GridReader reader)
        {
            var instance = (await reader.ReadAsync<TInstance>()).OrderBy(object => object.InstanceId).ToArray();
            var attributes = (await reader.ReadAsync<CustomAttributeValueDto>()).OrderBy(object => object.InstanceId).ToArray();
            var categories = (await reader.ReadAsync<CategoryDto>()).OrderBy(object => object.InstanceId).ToArray();

            var joinedData = instances
                .GroupJoin(attributes, o => o.InstanceId, i => i.InstanceId,
                    (inst, attrs) => new { Instance = inst, Attributes = attrs.ToCustomAttributes() })
                .GroupJoin(categories, o => o.InstanceId, i => i.InstanceId,
                    (inst, cats) => new { inst.Instance, inst.Attributes, Categories = cats });

            foreach (var inst in joinedData)
            {
                inst.Instance.CustomAttributes = inst.Attributes;
                inst.Instances.Categories = new CategoryCollection(inst.Categories.Select(c => new CategoryCollectionEntry
                {
                    CategoryInstance = c.CategoryName,
                    CategoryId = c.CategoryInstanceId
                }).ToArray();
            }

            return instance;
        }

        // 
        private class CategoryDto
        {
            public int InstanceId { get; set; }

            public int CategoryInstanceId { get; set; }
            public string CategoryName { get; set; }

            //
            public static IntergerCollection GetCategories(IEnumerable<CategoryDto> categories, int instanceId) =>
                new IntergerCollection(categories.Where(cat => cat.InstanceId == instanceId).Select(cat => cat.CategoryInstanceId));
        }
    }
}
