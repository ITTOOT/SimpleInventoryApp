using SimpleInventoryApp.Models.Base;
using SimpleInventoryApp.Models.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Services.IInterfaces
{
    // Interface
    public interface IInstanceRepository<TInstance>
        where TInstance : Instance
    {
        //Add required CRUD methods here
        Task<int> AddAsync(Task instance);
        Task<IEnumerable<TInstance>> GetAsync(params int[] instanceIds);
        Task<IEnumerable<TInstance>> FindAsync(ExactMatchQuery query);
    }

    // Exact match query
    public class ExactMatchQuery
    {
        /// <summary>
        /// [Performance Enhancement]
        /// Used to ensure that a match is found on a specific instance id
        /// </summary>
        public int? InstanceId { get; set; }
        public string InstanceName { get; set; }

        /// <summary>
        /// The categories the instance must have applied. Multiple categories apply as AND operations.
        /// For instance, if a search is performed with categories "X1" and "X2" then only instances 
        /// with X1 AND X2 will be returned.
        /// </summary>
        public string[] Categories { get; set; }

        /// <summary>
        /// The categories the instance must not have applied. Multiple categories apply as OR operations.
        /// For instance, if a search is performed with except categories "X1" and "X2" then only instances
        /// that do not contain "X1" OR "X2" will be returned.
        /// </summary>
        public string[] ExceptCategories { get; set; }

        /// <summary>
        /// The categories the instance must have applied. Multiple categories apply as AND operations.
        /// For instance, if a search is performed with category ids 1 and 2 then only instances 
        /// with category ids 1 AND 2 will be returned.
        /// </summary>
        public int[] CategoryIds { get; set; }

        /// <summary>
        /// The custom attributes and values that the instance must exactly match. Multiple
        /// attributes apply as AND operations.
        /// </summary>
        public CustomAttributes customAttributes { get; set; } = new CustomAttributes();
        public bool IncludeCustomAttributes => customAttributes?.Any() ?? false;
    }


}
