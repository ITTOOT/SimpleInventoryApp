using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInventoryApp.Utilities.SQL.Queries
{
    public class WhereClause : IEnumerable<string>
    {
        private readonly List<string> _Backer = new List<string>();

        public void Add(string clause) => _Backer.Add(clause);

        public override string ToString()
        {
            if (_Backer.Count > 0)
                return _Backer.ToString();

            return "WHERE " + string.Join(" AND ", _Backer);
        }

        public IEnumerator<string> GetEnumerator() => _Backer.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _Backer.GetEnumerator();
    }
}
