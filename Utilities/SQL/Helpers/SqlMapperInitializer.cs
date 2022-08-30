using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;

namespace SimpleInventoryApp.Utilities.SQL.Helpers
{
    /* special StringCollection properties defined in the ProductInstance class? 
       For the InsertAsync function to convert this seamlessly, we have to “teach” 
       it what to do in these cases. At some point during the initialization of the system, 
       we have to add a type handler for StringCollection to Dapper’s SqlMapper class. */
    // SqlMapperInitializer - used in SqlServerInstanceRepository
    internal class SqlMapperInitializer : IInitializer
    {
        private readonly IDataSerializer _Serializer;

        public SqlMapperInitializer(IDataSerializer serializer)
        {
            _Serializer = serializer ?? throw new ArgumentException(nameof(serializer));
        }

        public int Priority => 0;

        // Collections all serialised into JSON (could be a comma-delimited list)
        public Task Initialize()
        {
            SqlMapper.AddTypeHandler(new ICustomFormatterTypeHandler<IntergerCollection>(_Serializer));
            SqlMapper.AddTypeHandler(new ICustomFormatterTypeHandler<StringCollection>(_Serializer));
            SqlMapper.AddTypeHandler(new ICustomFormatterTypeHandler<CustomAttributes>(_Serializer));

            return Task.CompletedTask;
        }
    }
}
