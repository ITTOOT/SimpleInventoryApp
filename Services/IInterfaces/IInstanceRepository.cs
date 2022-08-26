using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Services.IInterfaces
{
    public interface IInstanceRepository<IInstance>
        where IInstance : Instance
    {
        Task<int> AddAsync(Task instance);
    }
}
