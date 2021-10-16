using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SSDTScaffolderLib.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ssdt = Microsoft.SqlServer.Dac.Model;

namespace SSDTScaffolderLib.Models
{
    public class Column
    {

        public Column()
        {

        }

        public string Name { get; internal set; }
        public bool IsPrimaryKey { get; internal set; }
        public bool IsForeignKey { get; internal set; }
        public bool IsNullable { get; internal set; }
        public bool IsIdentity { get; internal set; }
        public int Length { get; internal set; }

        public IDictionary<TypeFormat, string> DataTypes { get; internal set; }
        public IEnumerable<RelationshipIdentifier> ChildRelationships { get; internal set; }
        public IEnumerable<RelationshipIdentifier> ParentRelationships { get; internal set; }

        internal void SetProperties(ssdt.TSqlObject tSqlObject)
        {
            var sqlDataTypeName = tSqlObject.GetReferenced(ssdt.Column.DataType).ToList().First().Name.Parts.Last();
            this.DataTypes = DataTypeHelper.Instance.GetMap(TypeFormat.SqlServerDbType, sqlDataTypeName);
            this.IsNullable = ssdt.Column.Nullable.GetValue<bool>(tSqlObject);
            this.IsIdentity = ssdt.Column.IsIdentity.GetValue<bool>(tSqlObject);
            this.Length = ssdt.Column.Length.GetValue<int>(tSqlObject);
            //this.Precision = ssdt.Column.Precision.GetValue<int>(tSqlObject);
            //this.Scale = ssdt.Column.Scale.GetValue<int>(tSqlObject);
        }
    }
}
