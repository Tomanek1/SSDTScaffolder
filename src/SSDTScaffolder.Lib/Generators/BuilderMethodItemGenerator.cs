using SSDTScaffolderLib.Helpers;
using SSDTScaffolderLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDTScaffolderLib.Generators
{
    internal class BuilderMethodItemGenerator
    {
        string entityRelation = @"
                entity.HasOne(d => d.{0})
		            .WithMany(p => p.{1})
		            .HasForeignKey(x => x.{2})
		            .HasConstraintName(""{3}"");" + Environment.NewLine;

        public BuilderMethodItemGenerator()
        {
            entityRelation = entityRelation.ReplaceFirst(Environment.NewLine, "");
        }

        public string Generate(List<Column> columns, string tableName)
        {
            string cols = "";
            foreach (Column col in columns)
            {
                if (col.IsForeignKey)
                {

                    //cols += "entity.HasIndex(x => x." + col.Name + ");" + Environment.NewLine;

                    var parentName = col.ParentRelationships.First().TableOrView;
                    cols += string.Format(entityRelation, col.Name + "Navigation", tableName, col.Name, parentName + "_" + tableName);
                }
                if (col.DataTypes.TryGetValue(TypeFormat.SqlServerDbType, out string value) && value == "sql_variant")
                {
                    cols += "\t\t\t\tentity.Property(e => e.Value)" + Environment.NewLine +
                        "\t\t\t\t\t.HasColumnType(\"sql_variant\");" + Environment.NewLine;
                }
            }
            return cols;
        }
    }
}
