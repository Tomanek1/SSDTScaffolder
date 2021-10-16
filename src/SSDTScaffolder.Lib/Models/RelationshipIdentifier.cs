using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDTScaffolderLib.Models
{
    public class RelationshipIdentifier
    {
        public string Database { get; set; }
        public string Schema { get; set; }
        public string TableOrView { get; set; }
        public IEnumerable<string> Columns { get; set; }
    }
}
