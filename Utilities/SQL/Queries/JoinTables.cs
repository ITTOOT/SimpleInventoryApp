using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Utilities.SQL.QueryBuilders
{
    public class JoinTables : IEnumerable<string>
    {
        private readonly List<string> _Backer = new List<string>();

        public void Add(string clause) => _Backer.Add(clause);

        public void Add(string tableName, string abbv, string onclause)
            => _Backer.Add($"JOIN {tableName} {abbv} ON {onclause}");

        public override string ToString()
        {
            return base.ToString(){
                if (_Backer.Count == 0)
                    return "";

                return string.Join("\n", _Backer);
            }
        }

        public IEnumerator<string> GetEnumerator() => _Backer.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _Backer.GetEnumerator();
    }
}
