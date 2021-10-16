
using ssdt = Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SSDTScaffolderLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SSDTScaffolderLib.Helpers;

namespace SSDTScaffolderLib.Services
{
    class ColumnService
    {
        public Column CreateColumn(ssdt.TSqlObject tSqlObject, ssdt.TSqlObject tSqlTable, IEnumerable<ssdt.TSqlObject> primaryKeys, IDictionary<ssdt.TSqlObject, IEnumerable<ForeignKeyConstraintDefinition>> foreignKeys)
        {
            var column = new Column();
            column.Name = tSqlObject.Name.Parts.Last();
            var fullName = string.Join(".", tSqlObject.Name.Parts);

            column.IsPrimaryKey = primaryKeys.Any(p => string.Join(".", p.Name.Parts) == fullName);

            // Get relationships where this column is the child.
            IEnumerable<ForeignKeyConstraintDefinition> myForeignKeys;
            foreignKeys.TryGetValue(tSqlTable, out myForeignKeys);
            myForeignKeys = myForeignKeys ?? Enumerable.Empty<ForeignKeyConstraintDefinition>();
            column.ParentRelationships = from f in myForeignKeys
                                         where f.Columns.Any(c => c.Value == column.Name)
                                         select new RelationshipIdentifier
                                         {
                                             TableOrView = f.ReferenceTableName.BaseIdentifier != null ? f.ReferenceTableName.BaseIdentifier.Value : null,
                                             Schema = f.ReferenceTableName.SchemaIdentifier != null ? f.ReferenceTableName.SchemaIdentifier.Value : null,
                                             Database = f.ReferenceTableName.DatabaseIdentifier != null ? f.ReferenceTableName.DatabaseIdentifier.Value : null,
                                             Columns = f.ReferencedTableColumns.Select(c => c.Value)
                                         };
            column.IsForeignKey = column.ParentRelationships.Any();

            // Get relationships where this column is the parent.
            var childTables = from f in foreignKeys
                              where f.Value.Any(v => v.ReferenceTableName.BaseIdentifier.Value == tSqlTable.Name.Parts.Last()
                                && v.ReferencedTableColumns.Any(c => c.Value == column.Name))
                              select f;
            column.ChildRelationships = from t in childTables
                                        from r in t.Value
                                        let tableParts = t.Key.Name.Parts.Count()
                                        select new RelationshipIdentifier
                                        {
                                            TableOrView = t.Key.Name.Parts.Last(),
                                            Schema = tableParts > 1 ? t.Key.Name.Parts.ElementAt(tableParts - 2) : null,
                                            Database = tableParts > 2 ? t.Key.Name.Parts.ElementAt(tableParts - 3) : null,
                                            Columns = r.Columns.Select(c => c.Value)
                                        };


            if (tSqlObject.ObjectType.Name == "TableTypeColumn")
            {
                var sqlDataTypeName = tSqlObject.GetReferenced(ssdt.TableTypeColumn.DataType).ToList().First().Name.Parts.Last();
                column.IsNullable = ssdt.TableTypeColumn.Nullable.GetValue<bool>(tSqlObject);
                column.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, sqlDataTypeName);
                column.IsIdentity = ssdt.TableTypeColumn.IsIdentity.GetValue<bool>(tSqlObject);
                column.Length = ssdt.TableTypeColumn.Length.GetValue<int>(tSqlObject);
                //this.Precision = ssdt.TableTypeColumn.Precision.GetValue<int>(tSqlObject);
                //this.Scale = ssdt.TableTypeColumn.Scale.GetValue<int>(tSqlObject);
            }
            else
            {
                ssdt.ColumnType metaType = tSqlObject.GetMetadata<ssdt.ColumnType>(ssdt.Column.ColumnType);

                switch (metaType)
                {
                    case ssdt.ColumnType.Column:
                    case ssdt.ColumnType.ColumnSet:
                        column.SetProperties(tSqlObject);
                        break;
                    case ssdt.ColumnType.ComputedColumn:
                        var referenced = tSqlObject.GetReferenced().ToArray();
                        if (referenced.Length == 1)
                        {
                            var tSqlObjectReferenced = referenced[0];
                            column.SetProperties(tSqlObjectReferenced);
                        }
                        else if (referenced.Length > 1)
                        {
                            var rr = referenced.Where(a => a.ObjectType.Name.ToString() == "DataType").Single().Name.Parts.First();
                            column.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, rr.ToString());
                            column.IsNullable = ssdt.Column.Nullable.GetValue<bool>(tSqlObject);
                        }
                        else
                        {
                            var sqlDataTypeName = tSqlObject.GetReferenced().ToList();
                            column.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, "text");
                            column.IsNullable = ssdt.Column.Nullable.GetValue<bool>(tSqlObject);
                        }
                        break;
                }
            }
            return column;
        }


    }
}
