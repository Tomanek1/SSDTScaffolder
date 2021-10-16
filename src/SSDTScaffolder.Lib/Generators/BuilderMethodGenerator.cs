using SSDTScaffolderLib.Helpers;
using SSDTScaffolderLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDTScaffolderLib.Generators
{
    internal class BuilderMethodGenerator
    {
        BuilderMethodItemGenerator builderMethodItemGenerator;
        string builder = @"
            modelBuilder.Entity<{0}>(entity =>
            {{
{1}
            }});" + Environment.NewLine + Environment.NewLine;


        string viewProp = @"
                entity.HasNoKey();
                entity.ToView(""{0}"", ""{1}"");";




        private IEnumerable<Table> tables;
        private IEnumerable<View> views;

        public BuilderMethodGenerator(IEnumerable<Table> tables, IEnumerable<View> views)
        {
            this.tables = tables;
            this.views = views;
            builder = builder.ReplaceFirst(Environment.NewLine, "");
            viewProp = viewProp.ReplaceFirst(Environment.NewLine, "");
            builderMethodItemGenerator = new BuilderMethodItemGenerator();
        }

        public string GenerateBuilder()
        {
            string output = "";

            foreach (Table table in tables)
            {
                string cols = "";
                if (table.Schema != "dbo")
                    cols += "\t\t\t\tentity.ToTable(\"" + table.Name + "\", \"" + table.Schema + "\");" + Environment.NewLine;

                cols += builderMethodItemGenerator.Generate(table.Columns, table.Name);

                output += string.Format(builder, table.Name, cols);
            }

            foreach (View item in views)
            {
                string vals = string.Format(viewProp, item.Name, item.Schema);
                output += string.Format(builder, item.Name, vals);
            }

            return output;
        }

    }
}
