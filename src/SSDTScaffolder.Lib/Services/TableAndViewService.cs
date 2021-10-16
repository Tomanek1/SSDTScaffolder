﻿
using ssdt = Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SSDTScaffolderLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SSDTScaffolderLib.Services
{
    internal class TableAndViewService
    {
        ColumnService columnService = new ColumnService();
        internal Table CreateTable(ssdt.TSqlObject tSqlObject, IEnumerable<ssdt.TSqlObject> primaryKeys, Dictionary<ssdt.TSqlObject, IEnumerable<ForeignKeyConstraintDefinition>> foreignKeys)
        {
            var table = new Table();
            table.Schema = tSqlObject.Name.Parts.First();
            table.Name = tSqlObject.Name.Parts.Last();

            if (tSqlObject.ObjectType.Name == "Table")
            {
                // these properties are not valid on 'TableType'
                table.IsAutoGeneratedHistoryTable = ssdt.Table.IsAutoGeneratedHistoryTable.GetValue<bool>(tSqlObject);
                table.MemoryOptimized = ssdt.Table.MemoryOptimized.GetValue<bool>(tSqlObject);
            }

            table.Columns = new List<Column>();
            var sqlColumns = tSqlObject.ObjectType.Name == "TableType" ? tSqlObject.GetReferenced(ssdt.TableType.Columns) : tSqlObject.GetReferenced(ssdt.Table.Columns);
            foreach (var sqlColumn in sqlColumns)
            {
                var column = columnService.CreateColumn(sqlColumn, tSqlObject, primaryKeys, foreignKeys);
                table.Columns.Add(column);
            }
            return table;
        }

        internal View CreateView(ssdt.TSqlObject tSqlObject, IEnumerable<ssdt.TSqlObject> primaryKeys, Dictionary<ssdt.TSqlObject, IEnumerable<ForeignKeyConstraintDefinition>> foreignKeys)
        {
            var view = new View();
            view.Schema = tSqlObject.Name.Parts.First();
            view.Name = tSqlObject.Name.Parts.Last();

            var columns = new List<Column>();
            var sqlColumns = tSqlObject.GetReferenced(ssdt.View.Columns);
            foreach (var sqlColumn in sqlColumns)
            {
                var column = columnService.CreateColumn(sqlColumn, tSqlObject, primaryKeys, foreignKeys);
                columns.Add(column);
            }
            view.Columns = columns;
            return view;
        }
    }
}