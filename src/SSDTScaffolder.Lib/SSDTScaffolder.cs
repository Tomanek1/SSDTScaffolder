using SSDTScaffolderLib.Models;
using SSDTScaffolderLib.Helpers;
using SSDTScaffolderLib.Services;
using System;
using System.Collections.Generic;
using SSDTScaffolderLib.Scaffolders;
using SSDTScaffolderLib.Interface;
//using Microsoft.SqlServer.Dac.Model;

namespace SSDTScaffolderLib
{
    public class SSDTScaffolder
    {
        private SSDTService ssdtService;
        private PocoScaffolder pocoScaffolder;
        private DbContextScaffolder contextScaffolder;

        private List<string> SqlPaths { get; }

        /// <summary>
        /// Initializes new <see cref="SSDTScaffolder"/> instance
        /// </summary>
        /// <param name="sqlPaths">The paths to the *.sql files or folders</param>
        public SSDTScaffolder(params string[] sqlPaths)
        {
            this.SqlPaths = new List<string>();
            this.SqlPaths.AddRange(sqlPaths);
            ssdtService = new SSDTService(ScriptHelper.GetAllSqlFiles(sqlPaths));
        }

        public SSDTScaffolder(string projectPath)
        {
            this.SqlPaths = new List<string>();

            this.SqlPaths.AddRange(ScriptHelper.GetProjectFiles(projectPath));
            ssdtService = new SSDTService(this.SqlPaths);
        }

        public IEnumerable<Table> Tables
        {
            get
            {
                return ssdtService.GetTables();
            }
        }

        public IEnumerable<View> Views
        {
            get
            {
                return ssdtService.GetViews();
            }
        }

        public void ScaffoldPocoClasses(string targetDirectory, string targetNamespace)
        {
            pocoScaffolder = new PocoScaffolder(Tables, Views);
            pocoScaffolder.Invoke(targetDirectory, targetNamespace);
        }

        public void ScaffoldDbContext(string targetDirectory, string targetNamespace, string contextName)
        {
            contextScaffolder = new DbContextScaffolder(Tables, Views);
            contextScaffolder.Invoke(targetDirectory, targetNamespace, contextName);
        }
    }
}
