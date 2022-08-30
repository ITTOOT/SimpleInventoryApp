using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Utilities.SQL.QueryBuilders
{
    // Custom attributes search can provide false results because of the naive searches being conducted...
    // searching for a product that had 2 attributes that matched, will fail.
    // Instead, we get any instances that had 1, or both, of the attribute values (acting as an OR) operation.
    // This pivot table builder allows for the AND operation to occur
    public class KeyValuePivotTable
    {
        public string TableName { get; set; }
        public string PivotAlias { get; set; } = "PVT";
        public string InstanceIdColumnName { get; set; } = "[InstanceId]";
        public string ValueColumnName { get; set; } = "[Value]";
        public string KeyColumnName { get; set; } = "[Key]";

        public Dictionary<string, string> PivotValues { get; set; } = new Dictionary<string, string>();


        public string ToJoin()
        {
            string pivotColoumns = string.Join(", ", PivotValues.Keys.Select(k => "[" + k.Trim('[', ']') + "]"));

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"SELECT {InstanceIdColumnName}, {pivotColoumns}");
            builder.AppendLine($"FROM {TableName}");
            builder.AppendLine($"PIVOT (MIN({ValueColumnName}) FOR {KeyColumnName} IN ({pivotColoumns})) {PivotAlias}");

            return builder.ToString();
        }

        public string ToWhereClause(string joinAlias, BooleanJoin joiner = BooleanJoin.And)
        {
            string BOOL_JOINER = "AND";
            if (joiner == BooleanJoin.Or)
                BOOL_JOINER = "OR";

            var stringifiedValues = PivotValues
                                        .Select(kv => new
                                        {
                                            key = "[" + kv.Key.Trim('[', ']') + "]",
                                            kv.Value
                                        })
                                        // TODO: MUST FIGURE OUT HOW TO PARAMETERISE THIS EFFECTIVLY; THIS IS NOT GOOD!!!
                                        .Select(kv => $"{joinAlias}.{kv.key} = '{kv.Value?.Replace("'", "''")}'");

                                    return string.Join($" {BOOL_JOINER} ", stringifiedValues);
        }

        public enum Booleanjoin
        {
            And,
            Or
        }
    }
}
