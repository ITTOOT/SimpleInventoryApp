using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Utilities.SQL.Executor
{
    // ISqlExecutor
    public interface ISqlExecutor<TInstance> : ISqlExecutor
        where TInstance : Instance
    {
        string InstanceTableName { get; }
        string InstanceCustomAttributesTableName { get; }
        string InstanceCategoriesTableName { get; }

        Task WriteCategories(SqlConnection sqlConn, int instanceId, IntergerCollection categories, IDbTransaction transaction);
        Task WriteCustomAttributes(SqlConnection sqlConn, int instanceId, CustomAttributes attributes, IDbTransaction transaction);
        Task<IEnumerable<TInstance>> FindInner(string query, object parameters = null);
        Task<IEnumerable<TInstance>> FindInnerFilter(string filterQuery, object parameters = null);
    }

    // Constructor
    public interface ISqlExecutor
    {
        Task ExecuteAsync(Func<SqlConnection, SqlTransaction, Task> command);
        Task<T> ExecuteAsync<T>(Func<SqlConnection, SqlTransaction, Task<T>> command);
    }
}
