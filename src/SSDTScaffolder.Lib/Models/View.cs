using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Linq;
using ssdt = Microsoft.SqlServer.Dac.Model;

namespace SSDTScaffolderLib.Models
{
    public class View
    {
        public View()
        {

        }

        public string Name { get; set; }
        public string Schema { get; internal set; }

        public IEnumerable<Column> Columns { get; set; }
    }
}