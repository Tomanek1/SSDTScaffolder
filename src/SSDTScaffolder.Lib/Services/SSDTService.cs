using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SSDTScaffolderLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ssdt = Microsoft.SqlServer.Dac.Model;

namespace SSDTScaffolderLib.Services
{
    internal class SSDTService
    {
        private IEnumerable<string> sqlFiles;
        private IEnumerable<Table> tables;
        private IEnumerable<View> views;
        private IEnumerable<string> indexes;
        private ssdt.SqlServerVersion sqlServerVersion = ssdt.SqlServerVersion.Sql110;
        private TableAndViewService tableService;

        public SSDTService(IEnumerable<string> sqlFiles)
        {
            this.sqlFiles = sqlFiles;
            this.tableService = new TableAndViewService();
        }

        internal IEnumerable<Table> GetTables()
        {
            LoadModels();
            return tables;
        }

        public void LoadModels()
        {
            if (tables != null || views != null)
                return;//Models already loaded

            var model = new ssdt.TSqlModel(sqlServerVersion, new ssdt.TSqlModelOptions());
            foreach (var procFile in sqlFiles)
            {
                model.AddObjects(File.ReadAllText(procFile));
            }

            var sqlTables = model.GetObjects(ssdt.DacQueryScopes.UserDefined, ssdt.Table.TypeClass);
            var sqlViews = model.GetObjects(ssdt.DacQueryScopes.UserDefined, ssdt.View.TypeClass);
            var sqlIndexes = model.GetObjects(ssdt.DacQueryScopes.UserDefined, ssdt.Index.TypeClass);
            var primaryKeys = model.GetObjects(ssdt.DacQueryScopes.UserDefined, ssdt.PrimaryKeyConstraint.TypeClass).Select(o => o.GetReferenced().Where(r => r.ObjectType.Name == "Column")).SelectMany(c => c);
            var foreignKeyDictionaries = new[] { GetForeignKeys(sqlTables), GetForeignKeys(sqlViews) };
            var foreignKeys = foreignKeyDictionaries
                .SelectMany(x => x)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            tables = sqlTables.Select(sqlTable => tableService.CreateTable(sqlTable, primaryKeys, foreignKeys)).OrderBy(a => a.Name).ToList();
            views = sqlViews.Select(sqlView => tableService.CreateView(sqlView, primaryKeys, foreignKeys)).OrderBy(a => a.Name).ToList();
            indexes = sqlIndexes.Select(sqlIndex => sqlIndex.GetScript().Replace(Environment.NewLine, string.Empty)).ToList();
        }

        internal IEnumerable<View> GetViews()
        {
            LoadModels();
            return views;
        }

        public IDictionary<ssdt.TSqlObject, IEnumerable<ForeignKeyConstraintDefinition>> GetForeignKeys(IEnumerable<ssdt.TSqlObject> objects)
        {
            return objects.Select(obj =>
            {
                TSqlFragment fragment;
                TSqlModelUtils.TryGetFragmentForAnalysis(obj, out fragment);
                var foreignKeyConstraintVisitor = new ForeignKeyConstraintVisitor();
                fragment.Accept(foreignKeyConstraintVisitor);
                return new { obj, foreignKeyConstraintVisitor.Nodes };
            }).ToDictionary(key => key.obj, val => val.Nodes.AsEnumerable());
        }
    }
}
