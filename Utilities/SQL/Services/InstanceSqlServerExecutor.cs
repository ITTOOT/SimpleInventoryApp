using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleInventoryApp.Models.Standard;

namespace SimpleInventoryApp.Utilities.SQL.Executor
{
    public class InstanceSqlServerExecutor<TInstance> : ISqlExecutor<TInstance>
        where TInstance : InstanceSqlServerExecutor, new()
    {
        private readonly ISqlExecutor _Executor;
        private readonly InstanceReader<TInstance> _Reader;

        // Constructor to pull attributes and store results locally
        private InstanceSqlServerExecutor(ISqlExecutor executor)
        {
            _Executor = executor ?? throw new ArgumentNullException(nameof(executor));
            InstanceTableName = DapperExtensions.GetTableName<TInstance>();
            InstanceCustomAttributesTableName = DapperExtensions.GetCustomAttributesTableName<TInstance>();
            InstanceCategoriesTableName = DapperExtensions.GetCategoriesTableName<TInstance>();
            _Reader = new InstanceReader<TInstance>();
        }

        // Locals
        public string InstanceTableName { get; }
        public string InstanceCustomAttributesTableName { get; }
        public string InstanceCategoriesTableName { get; }

        // Command 
        public Task ExecuteAsync(Func<SqlConnection, SqlTransaction, Task> command)
            => _Executor.ExecuteAsync(command);

        // Command 
        public Task<T> ExecuteAsync(Func<SqlConnection, SqlTransaction, Task> command)
            => _Executor.ExecuteAsync<T>(command);

        // Find 
        public async Task<IEnumerable<TInstance>> FindInner(string query, object parameters = null)
            => await _Executor.ExecuteAsync(async (sqlConn, transaction) =>
            {
                using (var multi = await sqlConn.QueryMultipleAsync(query, parameters, transaction: transaction))
                    return await _Reader.ReadInstances(multi);
            });

        // Find and generate new find
        public async Task<IEnumerable<TInstance>> FindInnerFilter(string filterQuery, object parameter = null)
            => _Executor.ExecuteAsync(async (sqlConn, transaction) =>
            {
                using (var multi = await sqlConn.QueryMultipleAsync(GenerateFindQuery(filterQuery), parameters, transaction: transaction))
                    return await _Reader.ReadInstances(multi);
            });

        //////////////////////////////// SQL ////////////////////////////////
        // Be extremely cautious to ensure that no user data ends up in the SQL without being parameterized.
        //
        // Write categories (C) into table
        public async Task WriteCategories(SqlConnection sqlConn, int instanceId, IntegerCollection categories, IDbTransaction transaction)
        {
            // Delete rows instead of overwriting, then add duplicate (optimise this)
            string SQL = $@"
            DELETE FROM {InstanceCategoriesTableName} WHERE [InstanceId] = @InstanceId;
            INSERT INTO {InstanceCategoriesTableName} ([InstanceId], [CategoryInstanceId])
                SELECT @InstanceId, C.Value
                FROM @Categories C;
        ";

            // Execute the above query
            await sqlConn.ExecuteAsync(SQL, new
            {
                InstanceId = instanceId,
                Categories = categories.ToSqlParameter()
            }, transaction: transaction);
        }

        // Write custom attributes (CA) into tables
        public async Task WriteCustomAttributes(SqlConnection sqlConn, int instanceId, CustomAttributes attributes, IDbTransaction transaction)
        {
            // Delete rows instead of overwriting, then add duplicate (optimise this)
            string SQL = $@"
            DELETE FROM {InstanceCustomAttributesTableName} WHERE [InstanceId] = @InstanceId;
            INSERT INTO {InstanceCustomAttributesTableName} ([InstanceId], [Key], [Value])
                SELECT @InstanceId, CA.[Key], CA.[Value]
                FROM @CustomAttributes CA";

            // Execute the above query
            await sqlConn.ExecuteAsync(SQL, new
            {
                InstanceId = instanceId,
                CustomAttributes = attributes.ToSqlParameter()
            }, transaction: transaction);
        }

        // Filter & find where categories (C) & custome attributes (CA) match the query
        // General filteruqery finds instance IDs first...
        // then several SELECT queries based on those results, return multiple result sets (very efficient)
        private string GenerateFindQuery(string filterQuery) => $@"
            DECLARE @Ids TABLE ([Id] INT NOT NULL)
            ;
            INSERT INTO @Ids ([Id])
            {filterQuery}
            ;
            SELECT *
            FROM {InstanceTableName}
            WHERE [InstanceId] IN (SELECT [Id] FROM @Ids)
            ;
            SELECT *
            FROM {InstanceCustomAttributesTableName}
            WHERE [InstanceId] IN (SELECT [Id] FROM @Ids)
            ;
            SELECT 
                INST_CAT.[InstanceId] AS [InstanceId],
                CAT.[InstanceId] AS [CategoryInstanceId],
                CAT.[Name] AS [CategoryName]
            FROM {InstanceCategoriesTableName} INST_CAT
            JOIN [Instances].[Categories] CAT
                ON CAT.[InstanceId] = INST_CAT.[CategoryInstanceId]
            WHERE INST_CAT.[InstanceId] IN (SELECT [Id] FROM @Ids)
            ;
        ";
    }
}
