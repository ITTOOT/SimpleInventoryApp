using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleInventoryApp.Models.Base;
using SimpleInventoryApp.Models.Standard;

namespace SimpleInventoryApp.Utilities.SQL.QueryBuilders
{
    public partial class SqlServerInstanceRepository<TInstance>
    {

        public virtual async Task<IEnumerable<TInstance>> FindAsync(ExactMatchQuery query)
        {
            WhereClause WHERE = new WhereClause();
            JoinTables CATEGORY_JOIN = new JoiunTables();

            // Categories sub-query joins
            if (query.Categories?.Any() ?? false)
            {
                string CAT_SQL = $@"
              SELECT COUNT(*) AS InstanceCount, INST_CAT.InstanceId AS InstanceId
              FROM {_Executor.InstanceTableName} INST_CAT
              JOIN {_Executor.InstanceCategoriesTableName} CAT_INST_CAT
                  ON CAT_INST_CAT.InstanceId = INST_CAT.InstanceId
              JOIN [Instances].[Categories] CAT_CAT
                  ON CAT_CAT.InstanceId = CAT_INST_CAT.CategoryInstanceId
              WHERE 
                  (CAT_CAT.Name IN (SELECT [Value] FROM @Categories))
              GROUP BY INST_CAT.InstanceId
          ";

                CATEGORY_JOIN.Add($"JOIN ({CAT_SQL}) CAT ON CAT.InstanceId = INST.InstanceId AND CAT.InstanceCount = @CategoryCount");
            }

            // Exception Categories sub-query joins
            if (query.ExceptCategories?.Any() ?? false)
            {
                string EXCEPT_SQL = $@"
              SELECT INST_EX.InstanceId
              FROM {_Executor.InstanceTableName} INST_EX
              JOIN {_Executor.InstanceCategoriesTableName} CAT_INST_EX
                  ON CAT_INST_EX.InstanceId = INST_EX.InstanceId
              JOIN [Instances].[Categories] CAT_EX
                  ON CAT_EX.InstanceId = CAT_INST_EX.CategoryInstanceId
              WHERE 
                  (CAT_EX.Name IN (SELECT [Value] FROM @ExceptedCategories))
          ";
                // Check if an entry can be added to the class based upon the properties set in the query
                WHERE.Add($"(INST.InstanceId NOT IN ({EXCEPT_SQL}))");
            }

            // Category IDs sub-query joins
            if (query.CategoryIds?.Any() ?? false)
            {
                string CAT_SQL = $@"
              SELECT COUNT(*) AS InstanceCount, INST_CATID.InstanceId AS InstanceId
              FROM {_Executor.InstanceTableName} INST_CATID
              JOIN {_Executor.InstanceCategoriesTableName} CAT_INST_CATID
                  ON CAT_INST_CATID.InstanceId = INST_CATID.InstanceId
              WHERE 
                  (CAT_INST_CATID.CategoryInstanceId IN (SELECT [Value] FROM @CategoryIds))
              GROUP BY INST_CATID.InstanceId
          ";

                CATEGORY_JOIN.Add($"JOIN ({CAT_SQL}) CATID ON CATID.InstanceId = INST.InstanceId AND CATID.InstanceCount = @CategoryIdCount");
            }

            // Add the instance name to INST.name
            if (!string.IsNullOrWhiteSpace(query.InstanceName))
                // Check if an entry can be added to the class based upon the properties set in the query
                WHERE.Add("(INST.Name = @InstanceName OR @InstanceName IS NULL)");
            if (query.InstanceId != null)
                // Check if an entry can be added to the class based upon the properties set in the query
                WHERE.Add("(INST.InstanceId = @InstanceId)");

            // Custom attributes
            if (query.IncludeCustomAttributes && (query.CustomAttributes?.Any() ?? false))
            {
                // Check for custom attributes and form pivot table accordingly otherwise optomise
                KeyValuePivotTable pivot = new KeyValuePivotTable
                {
                    TableName = _Executor.InstanceCustomAttributesTableName,
                    PivotValues = query.CustomAttributes
                };

                // Check if an entry can be added to the class based upon the properties set in the query
                WHERE.Add($"({pivot.ToWhereClause("CUST_ATTR")})");


                string SQL = $@"
              SELECT DISTINCT INST.InstanceId
              FROM {_Executor.InstanceTableName} INST
              JOIN ({pivot.ToJoin()}) CUST_ATTR
                  ON CUST_ATTR.InstanceId = INST.InstanceId
              {CATEGORY_JOIN}
              {WHERE}
          ";

                return await _Executor.FindInnerFilter(SQL, new
                {
                    Categories = new StringCollection(query.Categories ?? new string[] { }).ToSqlParameter(),
                    CategoryCount = query.Categories?.Count() ?? 0,
                    ExceptedCategories = new StringCollection(query.ExceptCategories ?? new string[] { }).ToSqlParameter(),
                    CategoryIds = new IntegerCollection(query.CategoryIds ?? new int[] { }).ToSqlParameter(),
                    CategoryIdCount = query.CategoryIds?.Count() ?? 0,
                    query.InstanceName,
                    CustomAttributes = new CustomAttributes(query.CustomAttributes ?? new Dictionary<string, string>()).ToSqlParameter()
                });
            }
            else
            {
                return await _Executor.FindInnerFilter($@"
              SELECT DISTINCT INST.InstanceId
              FROM {_Executor.InstanceTableName} INST
              {CATEGORY_JOIN}
              {WHERE}
          ", new
                {
                    Categories = new StringCollection(query.Categories ?? new string[] { }).ToSqlParameter(),
                    CategoryCount = query.Categories?.Count() ?? 0,
                    ExceptedCategories = new StringCollection(query.ExceptCategories ?? new string[] { }).ToSqlParameter(),
                    CategoryIds = new IntegerCollection(query.CategoryIds ?? new int[] { }).ToSqlParameter(),
                    CategoryIdCount = query.CategoryIds?.Count() ?? 0,
                    query.InstanceName,
                });0
            }
        }

    }
}
