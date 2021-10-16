using SSDTScaffolderLib.Helpers;
using SSDTScaffolderLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDTScaffolderLib.Generators
{
    internal class PocoGenerator
    {
        private IEnumerable<Table> tables;
        private IEnumerable<View> views;

        private string outputString = @"
using System;
using System.Collections.Generic;

namespace {0}
{{
    public partial class {1}
    {{
        {2}
        {3}
    }}
}}
";


        private string outputProperty = @"public {0} {1} {{ get; set; }}" + Environment.NewLine + "\t\t";
        private string outputPropertyObjects = @"public virtual {0} {1} {{ get; set; }}" + Environment.NewLine + "\t\t";
        private string outputPropertyObjectCollection = @"public virtual ICollection<{0}> {1} {{ get; set; }}" + Environment.NewLine + "\t\t";

        public PocoGenerator(IEnumerable<Table> tables, IEnumerable<View> views)
        {
            this.tables = tables;
            this.views = views;
            outputString = outputString.ReplaceFirst(Environment.NewLine, "");
        }

        internal IEnumerable<(string, string)> Generate(string targetDirectory, string targetNamespace)
        {
            foreach (Table table in tables)
            {

                string outputProprs = "";
                string outputPropObjs = "";
                foreach (var column in table.Columns)
                {
                    var datatype = column.IsNullable ? column.DataTypes[TypeFormat.DotNetFrameworkNullableType] : column.DataTypes[TypeFormat.DotNetFrameworkNotNullableType];
                    outputProprs += string.Format(outputProperty, datatype, column.Name);

                    if (column.ParentRelationships.Any())
                    {
                        var tg = column.ParentRelationships.First();
                        outputPropObjs += string.Format(outputPropertyObjects, tg.TableOrView, column.Name + "Navigation");
                    }
                    if (column.ChildRelationships.Any())
                    {
                        var tg = column.ChildRelationships.First();
                        outputPropObjs += string.Format(outputPropertyObjectCollection, tg.TableOrView, tg.TableOrView);
                    }
                }

                string outContent = string.Format(outputString, targetNamespace, table.Name, outputProprs, outputPropObjs);
                yield return (table.Name, outContent);
            }

            foreach (View table in views)
            {
                string outputProprs = "";
                string outputPropObjs = "";
                foreach (var column in table.Columns)
                {
                    var datatype = column.IsNullable ? column.DataTypes[TypeFormat.DotNetFrameworkNullableType] : column.DataTypes[TypeFormat.DotNetFrameworkNotNullableType];
                    outputProprs += string.Format(outputProperty, datatype, column.Name);
                }

                string outContent = string.Format(outputString, targetNamespace, table.Name, outputProprs, outputPropObjs);
                yield return (table.Name, outContent);

            }
        }
    }
}
