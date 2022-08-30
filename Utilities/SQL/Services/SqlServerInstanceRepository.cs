using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Utilities.SQL.Services
{
    // Advanced searching for repository
    public partial class SqlServerInstanceRepository<TInstance> : IInstanceRepository<TInstance>
        where TInstance : TInstance, new()
    {
        protected readonly ISqlExecutor<TInstance> _Executor;


        public SqlServerInstanceRepository(ISqlExecutor<TInstance> executor)
        {
            _Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        // Add an instance into the system
        public async virtual Task<int> AddAsync(TInstance instance)
        {
            return await _Executor.ExecuteAsync(async (sqlConn, transaction) =>
            {
                instance.CreatedTimestamp = DateTime.Now;

                // Dapper.Contrib.InsertAsync - automatically parameterizes the properties and inserts the record into the table.
                // Record property names must be matched with the database column names.
                var id = await sqlConn.InsertAsync(instance, transaction: transaction);  // "Teach" type handler needed for TypeCollections

                // Write the attributes and categories using the SQL executor
                await _Executor.WriteCustomAttributes(sqlConn, id, instance.CustomAttributes, transaction);
                await _Executor.WriteCategories(sqlConn, id, instance.Categories, transaction);
                return id;
            });
        }

        // SELECT all IDs in the array, Bulk retrieve instances from the system - easier to go from multiples to one.
        public virtual Task<IEnumerable<TInstance>> GetAsync(params int[] instanceIds) => 
                            _Executor.Filter( $@"SELECT [Value] FROM @InstanceIds;",
                                new { instanceIds = new InstanceCollection(instanceIds).ToSqlParameter() });
    }
}
